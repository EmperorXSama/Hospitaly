using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Common.Domain;
using Hospitaly.Modules.Clinic.Application.Abstractions.Data;
using Hospitaly.Modules.Clinic.Domain.Clinic;
using Hospitaly.Modules.Clinic.Domain.Clinic.Entities;
using Hospitaly.Modules.Clinic.Domain.Clinic.Enum;
using Hospitaly.Modules.Clinic.Domain.Clinic.ValueObjects;
using ClinicDomain = Hospitaly.Modules.Clinic.Domain.Clinic.Clinic;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.CreateClinic;

internal sealed class CreateClinicCommandHandler(
    IClinicRepository clinicRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateClinicCommand, Guid>
{
    public async Task<ErrorOr<Guid>> Handle(CreateClinicCommand request, CancellationToken cancellationToken)
    {
        var audit = new AuditInfo(request.UserId, DateTime.UtcNow);

        var infoResult = ClinicInfo.Create(request.Name, null, request.Description, null);
        if (infoResult.IsError) return infoResult.Errors;

        var addressResult = ClinicAddress.Create(
            request.Street, request.City, request.Region, request.PostalCode, request.Country);
        if (addressResult.IsError) return addressResult.Errors;

        var contactResult = ClinicContactInfo.Create(request.Phone, request.Email, null);
        if (contactResult.IsError) return contactResult.Errors;

        var validityResult = LicenceValidityPeriod.Create(
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow.AddYears(5));
        if (validityResult.IsError) return validityResult.Errors;

        var licenseResult = OperatingLicense.Create(
            audit,
            "PENDING-" + Guid.NewGuid().ToString("N")[..8],
            "Pending",
            LicenseType.General,
            validityResult.Value,
            LicenceAdministrativeStatus.Active);
        if (licenseResult.IsError) return licenseResult.Errors;

        var monday = OperatingHours.Create(DayOfWeek.Monday, false, TimeSpan.FromHours(9), TimeSpan.FromHours(17));
        if (monday.IsError) return monday.Errors;

        var clinicResult = ClinicDomain.Create(
            infoResult.Value,
            addressResult.Value,
            licenseResult.Value,
            contactResult.Value,
            monday.Value,
            audit);
        if (clinicResult.IsError) return clinicResult.Errors;

        var clinic = clinicResult.Value;

        var ownershipEffectiveRange = new OwnershipEffectiveRange(
            Common.Domain.Common.ValueObjects.DateTimeRange.Create(
                DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(10)).Value);

        var ownershipResult = ClinicOwnerShip.Create(
            audit,
            request.UserId,
            OwnerShipType.SoleOwner,
            100m,
            ownershipEffectiveRange,
            OwnerShipStatus.Active);
        if (ownershipResult.IsError) return ownershipResult.Errors;

        clinic.ReAllocateOwnership([ownershipResult.Value], request.UserId, DateTimeOffset.UtcNow);

        clinicRepository.Insert(clinic);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return clinic.Id;
    }
}
