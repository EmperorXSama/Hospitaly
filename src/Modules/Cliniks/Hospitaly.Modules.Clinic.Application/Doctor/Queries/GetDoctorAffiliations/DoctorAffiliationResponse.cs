namespace Hospitaly.Modules.Clinic.Application.Doctor.Queries.GetDoctorAffiliations;

public sealed record PrivilegeResponse(
    string Type,
    DateTime GrantedAt,
    Guid GrantedBy);

public sealed record DoctorAffiliationResponse(
    Guid Id,
    Guid ClinicId,
    string? ClinicName,
    string Status,
    DateTime JoinedDate,
    DateTime? TerminatedDate,
    Guid? DepartmentId,
    List<PrivilegeResponse> Privileges);
