using Hospitaly.Common.Application.Abstraction.Messaging;
using ErrorOr;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Command.VerifyDoctorCredential;

public sealed record VerifyDoctorCredentialCommand(
    Guid DoctorId,
    Guid CredentialId,
    Guid UserId) : ICommand<Success>;
