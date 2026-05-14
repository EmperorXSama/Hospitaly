namespace Hospitaly.Modules.Clinic.Application.Clinic.Queries.GetClinicOperatingLicense;

public sealed record ClinicOperatingLicenseResponse(
    Guid Id,
    string LicenseNumber,
    string IssuingAuthority,
    string LicenseType,
    DateTimeOffset ValidityStart,
    DateTimeOffset? ValidityEnd,
    string AdministrativeStatus);
