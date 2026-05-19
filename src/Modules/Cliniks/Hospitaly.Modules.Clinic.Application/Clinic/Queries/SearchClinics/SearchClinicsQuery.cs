using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Common.Application;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Queries.SearchClinics;

public sealed record SearchClinicsQuery(
    string? SearchTerm,
    string? City,
    int Page,
    int PageSize) : IQuery<PaginatedResult<ClinicSummaryResponse>>;
