using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Common.Domain;
using Hospitaly.Modules.Clinic.Application.Abstractions.Data;
using Hospitaly.Modules.Clinic.Domain.Clinic;
using Hospitaly.Modules.Clinic.Domain.Clinic.Entities;
using Hospitaly.Modules.Clinic.Domain.Clinic.ValueObjects;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.ReplaceOperatingLicense;

internal sealed class ReplaceOperatingLicenseCommandHandler(
    IClinicRepository clinicRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<ReplaceOperatingLicenseCommand, Success>
{
    public async Task<ErrorOr<Success>> Handle(
        ReplaceOperatingLicenseCommand request,
        CancellationToken cancellationToken)
    {
        var clinic = await clinicRepository.GetByIdAsync(request.ClinicId, cancellationToken);
        if (clinic is null)
            return Error.NotFound(
                code: "ReplaceOperatingLicense.NotFound",
                description: $"Clinic with id {request.ClinicId} was not found.");

        var validityResult = LicenceValidityPeriod.Create(
            request.ValidityStart,
            request.ValidityEnd ?? request.ValidityStart.AddYears(5));
        if (validityResult.IsError)
            return validityResult.Errors;

        var audit = new AuditInfo(request.UserId, DateTimeOffset.UtcNow);

        var licenseResult = OperatingLicense.Create(
            audit,
            request.LicenseNumber,
            request.IssuingAuthority,
            request.LicenseType,
            validityResult.Value,
            request.AdministrativeStatus);
        if (licenseResult.IsError)
            return licenseResult.Errors;

        var updateResult = clinic.ReplaceOperatingLicense(licenseResult.Value, request.UserId, DateTimeOffset.UtcNow);
        if (updateResult.IsError)
            return updateResult.Errors;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
