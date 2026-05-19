namespace Hospitaly.Modules.Clinic.Application.Clinic.Queries.GetClinicSpecialties;

public sealed record ClinicSpecialtyResponse(
    Guid SpecialtyId,
    string SpecialtyName,
    bool IsActive,
    decimal? ConsultationFee);
