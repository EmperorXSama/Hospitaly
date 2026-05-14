namespace Hospitaly.Modules.Clinic.Application.Doctor.Queries.GetDoctorCredentials;

public sealed record DoctorCredentialResponse(
    Guid Id,
    string CredentialType,
    string IssuingAuthority,
    string DocumentNumber,
    string Status,
    DateTime? VerifiedAt,
    Guid? VerifiedBy,
    DateTime IssueDate,
    DateTime? ExpiryDate);
