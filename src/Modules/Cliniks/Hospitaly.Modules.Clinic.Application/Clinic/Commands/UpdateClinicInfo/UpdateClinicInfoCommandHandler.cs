using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Common.Domain;
using Hospitaly.Modules.Clinic.Application.Abstractions.Data;
using Hospitaly.Modules.Clinic.Domain.Clinic;
using Hospitaly.Modules.Clinic.Domain.Clinic.ValueObjects;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.UpdateClinicInfo;

internal sealed class UpdateClinicInfoCommandHandler(
    IClinicRepository clinicRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateClinicInfoCommand, Success>
{
    public async Task<ErrorOr<Success>> Handle(
        UpdateClinicInfoCommand request,
        CancellationToken cancellationToken)
    {
        var clinic = await clinicRepository.GetByIdAsync(request.ClinicId, cancellationToken);
        if (clinic is null)
            return Error.NotFound(
                code: "UpdateClinicInfo.NotFound",
                description: $"Clinic with id {request.ClinicId} was not found.");

        var infoResult = ClinicInfo.Create(request.Name, request.TradingName, request.Description, request.LogoUrl);
        if (infoResult.IsError)
            return infoResult.Errors;

        var updateResult = clinic.UpdateInfo(infoResult.Value, request.UserId, DateTimeOffset.UtcNow);
        if (updateResult.IsError)
            return updateResult.Errors;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
