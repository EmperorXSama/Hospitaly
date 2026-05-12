using ErrorOr;
using Hospitaly.Modules.Clinic.Domain.Patient.Enums;

namespace Hospitaly.Modules.Clinic.Domain.Patient.ValueObjects;

public sealed record PatientIdentity
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public DateOnly DateOfBirth { get; init; }
    public Gender Gender { get; init; }
    public string NationalId { get; init; }

    private PatientIdentity()
    {
    }

    private PatientIdentity(
        string firstName,
        string lastName,
        DateOnly dateOfBirth,
        Gender gender,
        string nationalId)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Gender = gender;
        NationalId = nationalId;
    }

    public static ErrorOr<PatientIdentity> Create(
        string firstName,
        string lastName,
        DateOnly dateOfBirth,
        Gender gender,
        string nationalId)
    {
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(firstName))
        {
            errors.Add(Error.Validation(
                code: "PatientIdentity.InvalidFirstName",
                description: "First name cannot be null, empty, or whitespace.",
                metadata: new Dictionary<string, object> { ["firstName"] = firstName }));
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            errors.Add(Error.Validation(
                code: "PatientIdentity.InvalidLastName",
                description: "Last name cannot be null, empty, or whitespace.",
                metadata: new Dictionary<string, object> { ["lastName"] = lastName }));
        }

        if (dateOfBirth > DateOnly.FromDateTime(DateTime.UtcNow))
        {
            errors.Add(Error.Validation(
                code: "PatientIdentity.InvalidDateOfBirth",
                description: "Date of birth cannot be in the future.",
                metadata: new Dictionary<string, object> { ["dateOfBirth"] = dateOfBirth }));
        }

        if (string.IsNullOrWhiteSpace(nationalId))
        {
            errors.Add(Error.Validation(
                code: "PatientIdentity.NationalIdRequired",
                description: "National ID cannot be null, empty, or whitespace.",
                metadata: new Dictionary<string, object> { ["nationalId"] = nationalId }));
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return new PatientIdentity(firstName, lastName, dateOfBirth, gender, nationalId);
    }
}
