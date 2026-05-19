namespace Hospitaly.Modules.Clinic.Application.Doctor.Queries.GetDoctorSpecialties;

public sealed record DoctorSpecialtyResponse(
    Guid SpecialtyId,
    string SpecialtyName,
    bool IsPrimary,
    string? CertificationNumber,
    DateTime CertifiedAt);
