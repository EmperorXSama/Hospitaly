using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Command.RevokeDoctorCredential;

public sealed record RevokeDoctorCredentialCommand(
    Guid DoctorId,
    Guid CredentialId,
    Guid UserId) : ICommand<Success>;
