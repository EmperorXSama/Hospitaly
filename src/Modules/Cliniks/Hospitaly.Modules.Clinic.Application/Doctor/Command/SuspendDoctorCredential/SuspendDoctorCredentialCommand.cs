using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Command.SuspendDoctorCredential;

public sealed record SuspendDoctorCredentialCommand(
    Guid DoctorId,
    Guid CredentialId,
    Guid UserId) : ICommand<Success>;
