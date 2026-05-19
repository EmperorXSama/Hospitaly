using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Command.DeactivateDoctor;

public sealed record DeactivateDoctorCommand(
    Guid DoctorId,
    Guid UserId) : ICommand<Success>;
