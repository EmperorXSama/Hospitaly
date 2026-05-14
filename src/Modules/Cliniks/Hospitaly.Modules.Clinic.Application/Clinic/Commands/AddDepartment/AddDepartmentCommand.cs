using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.AddDepartment;

public sealed record AddDepartmentCommand(
    Guid ClinicId,
    string Name,
    string Code,
    bool IsActive,
    Guid? ParentDepartmentId,
    Guid UserId) : ICommand<Guid>;
