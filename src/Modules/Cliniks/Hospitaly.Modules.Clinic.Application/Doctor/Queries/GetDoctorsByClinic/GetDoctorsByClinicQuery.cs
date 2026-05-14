using Hospitaly.Common.Application;
using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Queries.GetDoctorsByClinic;

public sealed record GetDoctorsByClinicQuery(
    Guid ClinicId,
    string? Status,
    int Page,
    int PageSize) : IQuery<PaginatedResult<DoctorAffiliationSummaryResponse>>;
