using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.UpdateClinicOwnerShare;

public sealed record UpdateClinicOwnerShareCommand(
    Guid ClinicId,
    Guid OwnershipId,
    decimal NewSharePercentage,
    Guid UserId) : ICommand<Success>;
