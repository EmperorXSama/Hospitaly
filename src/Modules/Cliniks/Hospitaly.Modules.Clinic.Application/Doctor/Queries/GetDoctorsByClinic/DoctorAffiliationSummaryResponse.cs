namespace Hospitaly.Modules.Clinic.Application.Doctor.Queries.GetDoctorsByClinic;

public sealed record DoctorAffiliationSummaryResponse(
    Guid DoctorId,
    string? Title,
    string DoctorStatus,
    Guid AffiliationId,
    string AffiliationStatus,
    DateTime JoinedDate,
    Guid? DepartmentId);
