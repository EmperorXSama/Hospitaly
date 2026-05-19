using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.UpdateClinicInfo;

public sealed record UpdateClinicInfoCommand(
    Guid ClinicId,
    string Name,
    string? TradingName,
    string Description,
    string? LogoUrl,
    Guid UserId) : ICommand<Success>;
