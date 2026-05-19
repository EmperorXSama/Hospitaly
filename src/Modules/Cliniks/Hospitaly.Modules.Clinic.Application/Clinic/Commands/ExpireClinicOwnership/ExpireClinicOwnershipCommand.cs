using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.ExpireClinicOwnership;

public sealed record ExpireClinicOwnershipCommand(
    Guid ClinicId,
    Guid OwnershipId,
    Guid UserId) : ICommand<Success>;
