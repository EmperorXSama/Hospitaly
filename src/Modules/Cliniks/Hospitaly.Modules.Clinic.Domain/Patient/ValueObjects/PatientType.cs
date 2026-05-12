using ErrorOr;
using Hospitaly.Modules.Clinic.Domain.Patient.Enums;

namespace Hospitaly.Modules.Clinic.Domain.Patient.ValueObjects;

public sealed record PatientType
{
    public VisitType Type { get; init; }
    public DateTime? RegistrationDate { get; init; }

    public bool IsRegistered => Type == VisitType.Registered;
    public bool IsWalkIn => Type == VisitType.WalkIn;

    private PatientType()
    {
    }

    private PatientType(VisitType type, DateTime? registrationDate)
    {
        Type = type;
        RegistrationDate = registrationDate;
    }

    public static ErrorOr<PatientType> Create(VisitType type, DateTime? registrationDate = null)
    {
        if (type == VisitType.Registered && registrationDate is null)
        {
            return Error.Validation(
                code: "PatientType.RegistrationDateRequired",
                description: "Registration date is required for registered patients.",
                metadata: new Dictionary<string, object>
                {
                    ["type"] = type.ToString(),
                    ["registrationDate"] = registrationDate
                });
        }

        return new PatientType(type, registrationDate);
    }
}
