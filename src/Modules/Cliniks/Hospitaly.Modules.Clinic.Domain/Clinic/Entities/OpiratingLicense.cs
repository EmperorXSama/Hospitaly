using ErrorOr;
using Hospitaly.Common.Domain;
using Hospitaly.Modules.Clinic.Domain.Clinic.Enum;
using Hospitaly.Modules.Clinic.Domain.Clinic.ValueObjects;

namespace Hospitaly.Modules.Clinic.Domain.Clinic.Entities;

public class OperatingLicense: Entity
{
    public string LicenseNumber { get; private set; }
    public string IssuingAuthority { get; set; }
    public LicenseType LicenseType { get; set; }
    public LicenceValidityPeriod ValidityPeriod { get; private set; }
    public LicenceAdministrativeStatus AdministrativeStatus { get; private set; }

    private OperatingLicense()
    {
    }

    private OperatingLicense(AuditInfo audit) : base(audit)
    {
    }

    public static ErrorOr<OperatingLicense> Create(
        AuditInfo audit,
        string licenseNumber,
        string issuingAuthority,
        LicenseType licenseType,
        LicenceValidityPeriod validityPeriod,
        LicenceAdministrativeStatus administrativeStatus)
    {
        if (string.IsNullOrWhiteSpace(licenseNumber))
            return Error.Validation("OperatingLicense.InvalidNumber", "License number cannot be empty.");

        if (string.IsNullOrWhiteSpace(issuingAuthority))
            return Error.Validation("OperatingLicense.InvalidAuthority", "Issuing authority cannot be empty.");

        return new OperatingLicense(audit)
        {
            LicenseNumber = licenseNumber,
            IssuingAuthority = issuingAuthority,
            LicenseType = licenseType,
            ValidityPeriod = validityPeriod,
            AdministrativeStatus = administrativeStatus
        };
    }
    
    
    
    public bool IsOperational =>
        AdministrativeStatus == LicenceAdministrativeStatus.Active &&
        ValidityPeriod.GetStatus(DateTimeOffset.UtcNow) != (LicenceValidityStatus.Expired | LicenceValidityStatus.NotStarted);
}