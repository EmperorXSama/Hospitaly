using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Modules.Clinic.Domain.Clinic.Enum;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.ReAllocateClinicOwnership;

public sealed record ReAllocateClinicOwnershipCommand(
    Guid ClinicId,
    List<OwnerInput> Owners,
    Guid UserId) : ICommand<Success>;

public sealed record OwnerInput(
    Guid? OwnershipId,
    Guid OwnerId,
    OwnerShipType OwnerShipType,
    decimal SharePercentage,
    DateTimeOffset EffectiveStart,
    DateTimeOffset? EffectiveEnd,
    OwnerShipStatus Status);
