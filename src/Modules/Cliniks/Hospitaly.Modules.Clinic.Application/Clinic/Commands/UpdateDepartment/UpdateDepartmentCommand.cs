using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.UpdateDepartment;

public sealed record UpdateDepartmentCommand(
    Guid ClinicId,
    Guid DepartmentId,
    string Name,
    string Code,
    Guid? ParentDepartmentId,
    Guid UserId) : ICommand<Success>;
