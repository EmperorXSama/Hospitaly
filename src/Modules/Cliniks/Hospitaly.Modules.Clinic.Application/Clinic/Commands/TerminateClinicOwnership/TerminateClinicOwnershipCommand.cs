using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.TerminateClinicOwnership;

public sealed record TerminateClinicOwnershipCommand(
    Guid ClinicId,
    Guid OwnershipId,
    Guid UserId) : ICommand<Success>;
