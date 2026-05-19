using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.SetDepartmentActiveState;

public sealed record SetDepartmentActiveStateCommand(
    Guid ClinicId,
    Guid DepartmentId,
    bool IsActive,
    Guid UserId) : ICommand<Success>;
