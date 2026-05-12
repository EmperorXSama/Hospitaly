namespace Hospitaly.Modules.Clinic.Domain.Clinic.Enum;

[Flags]
public enum LicenceValidityStatus  
{
    NotStarted,
    Active,
    ExpiringSoon,
    Expired
}