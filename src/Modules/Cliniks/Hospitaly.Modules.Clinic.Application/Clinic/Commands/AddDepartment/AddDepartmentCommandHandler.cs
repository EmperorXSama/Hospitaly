using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Modules.Clinic.Application.Abstractions.Data;
using Hospitaly.Modules.Clinic.Domain.Clinic;
using Hospitaly.Modules.Clinic.Domain.Clinic.Entities;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.AddDepartment;

internal sealed class AddDepartmentCommandHandler(
    IClinicRepository clinicRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<AddDepartmentCommand, Guid>
{
    public async Task<ErrorOr<Guid>> Handle(
        AddDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        var clinic = await clinicRepository.GetByIdAsync(request.ClinicId, cancellationToken);
        if (clinic is null)
            return Error.NotFound(
                code: "AddDepartment.NotFound",
                description: $"Clinic with id {request.ClinicId} was not found.");

        var departmentResult = Department.Create(
            request.Name,
            request.Code,
            request.IsActive,
            request.ClinicId,
            request.ParentDepartmentId,
            request.UserId,
            DateTime.UtcNow);
        if (departmentResult.IsError)
            return departmentResult.Errors;

        clinic.AddDepartment(departmentResult.Value);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return departmentResult.Value.Id;
    }
}
