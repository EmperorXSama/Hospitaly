using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Common.Domain;
using Hospitaly.Modules.Clinic.Application.Abstractions.Data;
using Hospitaly.Modules.Clinic.Domain.Clinic;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.UpdateDepartment;

internal sealed class UpdateDepartmentCommandHandler(
    IClinicRepository clinicRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateDepartmentCommand, Success>
{
    public async Task<ErrorOr<Success>> Handle(
        UpdateDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        var clinic = await clinicRepository.GetByIdAsync(request.ClinicId, cancellationToken);
        if (clinic is null)
            return Error.NotFound(
                code: "UpdateDepartment.NotFound",
                description: $"Clinic with id {request.ClinicId} was not found.");

        var result = clinic.UpdateDepartment(
            request.DepartmentId,
            request.Name,
            request.Code,
            request.ParentDepartmentId,
            request.UserId,
            DateTimeOffset.UtcNow);
        if (result.IsError)
            return result.Errors;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
