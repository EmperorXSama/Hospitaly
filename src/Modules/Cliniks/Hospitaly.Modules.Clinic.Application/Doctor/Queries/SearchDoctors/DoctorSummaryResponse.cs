namespace Hospitaly.Modules.Clinic.Application.Doctor.Queries.SearchDoctors;

public sealed record DoctorSummaryResponse(
    Guid Id,
    string Status,
    string? Title);
