using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Command.ReactivateDoctorCredential;

public sealed record ReactivateDoctorCredentialCommand(
    Guid DoctorId,
    Guid CredentialId,
    Guid UserId) : ICommand<Success>;
