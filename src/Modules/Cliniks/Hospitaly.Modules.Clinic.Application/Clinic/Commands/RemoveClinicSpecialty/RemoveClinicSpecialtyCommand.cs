using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.RemoveClinicSpecialty;

public sealed record RemoveClinicSpecialtyCommand(
    Guid ClinicId,
    Guid SpecialtyId,
    Guid UserId) : ICommand<Success>;
