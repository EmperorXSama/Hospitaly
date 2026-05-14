using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Command.ActivateDoctor;

public sealed record ActivateDoctorCommand(
    Guid DoctorId,
    Guid UserId) : ICommand<Success>;
