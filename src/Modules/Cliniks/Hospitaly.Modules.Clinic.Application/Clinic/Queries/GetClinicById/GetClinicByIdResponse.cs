namespace Hospitaly.Modules.Clinic.Application.Clinic.Queries.GetClinicById;

public sealed record ClinicDetailResponse(
    Guid Id,
    string Name,
    string? TradingName,
    string Description,
    string? LogoUrl,
    string Street,
    string City,
    string? Region,
    string? PostalCode,
    string Country,
    double? Latitude,
    double? Longitude,
    string? Phone,
    string? Email,
    string? Website,
    string LicenseNumber,
    string IssuingAuthority,
    string LicenseType,
    DateTimeOffset LicenseValidityStart,
    DateTimeOffset? LicenseValidityEnd,
    string LicenseAdministrativeStatus);
