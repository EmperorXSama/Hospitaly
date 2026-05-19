namespace Hospitaly.Modules.Clinic.Application.Doctor.Queries.GetDoctorById;

public sealed record DoctorCredentialItem(
    Guid Id,
    string CredentialType,
    string IssuingAuthority,
    string DocumentNumber,
    string Status,
    DateTime? VerifiedAt,
    Guid? VerifiedBy,
    DateTime IssueDate,
    DateTime? ExpiryDate);

public sealed record DoctorSpecialtyItem(
    Guid SpecialtyId,
    string SpecialtyName,
    bool IsPrimary,
    string? CertificationNumber,
    DateTime CertifiedAt);

public sealed record PrivilegeItem(
    string Type,
    DateTime GrantedAt,
    Guid GrantedBy);

public sealed record DoctorAffiliationItem(
    Guid Id,
    Guid ClinicId,
    string? ClinicName,
    string Status,
    DateTime JoinedDate,
    DateTime? TerminatedDate,
    Guid? DepartmentId,
    List<PrivilegeItem> Privileges);

public sealed record DoctorDetailResponse(
    Guid Id,
    string Status,
    string? Title,
    string? Bio,
    string? AvatarUrl,
    Guid CreatedBy,
    DateTime CreatedOnUtc,
    List<DoctorCredentialItem> Credentials,
    List<DoctorSpecialtyItem> Specialties,
    List<DoctorAffiliationItem> Affiliations);
