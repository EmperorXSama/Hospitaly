using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.TransferClinicOwnershipToUser;

public sealed record TransferClinicOwnershipToUserCommand(
    Guid ClinicId,
    Guid FromOwnershipId,
    string TargetOwnerIdentityId,
    string OwnerShipType,
    decimal PercentageToTransfer,
    DateTimeOffset EffectiveStart,
    Guid UserId) : ICommand<Success>;
