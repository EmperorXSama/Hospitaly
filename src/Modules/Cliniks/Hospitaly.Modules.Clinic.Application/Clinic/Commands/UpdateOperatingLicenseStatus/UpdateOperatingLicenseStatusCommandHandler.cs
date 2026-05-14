using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Common.Domain;
using Hospitaly.Modules.Clinic.Application.Abstractions.Data;
using Hospitaly.Modules.Clinic.Domain.Clinic;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.UpdateOperatingLicenseStatus;

internal sealed class UpdateOperatingLicenseStatusCommandHandler(
    IClinicRepository clinicRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateOperatingLicenseStatusCommand, Success>
{
    public async Task<ErrorOr<Success>> Handle(
        UpdateOperatingLicenseStatusCommand request,
        CancellationToken cancellationToken)
    {
        var clinic = await clinicRepository.GetByIdAsync(request.ClinicId, cancellationToken);
        if (clinic is null)
            return Error.NotFound(
                code: "UpdateOperatingLicenseStatus.NotFound",
                description: $"Clinic with id {request.ClinicId} was not found.");

        var updateResult = clinic.UpdateOperatingLicenseStatus(
            request.AdministrativeStatus, request.UserId, DateTimeOffset.UtcNow);
        if (updateResult.IsError)
            return updateResult.Errors;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
