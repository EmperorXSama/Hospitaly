using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Command.AddDoctorCredential;

public sealed record AddDoctorCredentialCommand(
    Guid DoctorId,
    string CredentialType,
    string IssuingAuthority,
    string DocumentNumber,
    DateTime IssueDate,
    DateTime ExpiryDate,
    Guid UserId) : ICommand<Guid>;
