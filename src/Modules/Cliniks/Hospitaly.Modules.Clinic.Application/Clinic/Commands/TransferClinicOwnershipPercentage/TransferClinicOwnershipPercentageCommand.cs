using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.TransferClinicOwnershipPercentage;

public sealed record TransferClinicOwnershipPercentageCommand(
    Guid ClinicId,
    Guid FromOwnershipId,
    decimal RetainedPercentage,
    List<TransferInput> Transfers,
    Guid UserId) : ICommand<Success>;

public sealed record TransferInput(Guid OwnershipId, decimal SharePercentage);
