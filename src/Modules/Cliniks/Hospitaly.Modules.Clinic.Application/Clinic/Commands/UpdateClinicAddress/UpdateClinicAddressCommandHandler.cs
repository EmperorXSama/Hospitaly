using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Common.Domain;
using Hospitaly.Modules.Clinic.Application.Abstractions.Data;
using Hospitaly.Modules.Clinic.Domain.Clinic;
using Hospitaly.Modules.Clinic.Domain.Clinic.ValueObjects;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.UpdateClinicAddress;

internal sealed class UpdateClinicAddressCommandHandler(
    IClinicRepository clinicRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateClinicAddressCommand, Success>
{
    public async Task<ErrorOr<Success>> Handle(
        UpdateClinicAddressCommand request,
        CancellationToken cancellationToken)
    {
        var clinic = await clinicRepository.GetByIdAsync(request.ClinicId, cancellationToken);
        if (clinic is null)
            return Error.NotFound(
                code: "UpdateClinicAddress.NotFound",
                description: $"Clinic with id {request.ClinicId} was not found.");

        Coordinates? coordinates = null;
        if (request.Latitude.HasValue && request.Longitude.HasValue)
        {
            var coordsResult = Coordinates.Create(request.Latitude.Value, request.Longitude.Value);
            if (coordsResult.IsError)
                return coordsResult.Errors;
            coordinates = coordsResult.Value;
        }

        var addressResult = ClinicAddress.Create(
            request.Street, request.City, request.Region, request.PostalCode, request.Country, coordinates);
        if (addressResult.IsError)
            return addressResult.Errors;

        var updateResult = clinic.UpdateAddress(addressResult.Value, request.UserId, DateTimeOffset.UtcNow);
        if (updateResult.IsError)
            return updateResult.Errors;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
