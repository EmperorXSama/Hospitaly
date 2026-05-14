using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.UpdateClinicContactInfo;

public sealed record UpdateClinicContactInfoCommand(
    Guid ClinicId,
    string? Phone,
    string? Email,
    string? Website,
    Guid UserId) : ICommand<Success>;
