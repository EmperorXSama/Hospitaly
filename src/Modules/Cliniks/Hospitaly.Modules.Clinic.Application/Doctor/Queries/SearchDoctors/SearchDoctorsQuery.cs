using Hospitaly.Common.Application;
using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Queries.SearchDoctors;

public sealed record SearchDoctorsQuery(
    string? SearchTerm,
    Guid? SpecialtyId,
    Guid? ClinicId,
    string? Status,
    int Page,
    int PageSize) : IQuery<PaginatedResult<DoctorSummaryResponse>>;
