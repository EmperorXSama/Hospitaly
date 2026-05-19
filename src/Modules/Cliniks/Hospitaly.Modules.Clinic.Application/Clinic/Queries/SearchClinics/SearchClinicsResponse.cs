namespace Hospitaly.Modules.Clinic.Application.Clinic.Queries.SearchClinics;

public sealed record ClinicSummaryResponse(
    Guid Id,
    string Name,
    string? TradingName,
    string City,
    string Country);
