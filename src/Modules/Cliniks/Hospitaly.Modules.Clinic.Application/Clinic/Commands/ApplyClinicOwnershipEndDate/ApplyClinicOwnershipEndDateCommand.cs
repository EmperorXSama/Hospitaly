using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.ApplyClinicOwnershipEndDate;

public sealed record ApplyClinicOwnershipEndDateCommand(
    Guid ClinicId,
    Guid OwnershipId,
    DateTimeOffset EffectiveUntil,
    Guid UserId) : ICommand<Success>;
