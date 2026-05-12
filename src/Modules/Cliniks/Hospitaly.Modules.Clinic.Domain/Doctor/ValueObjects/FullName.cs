using ErrorOr;

namespace Hospitaly.Modules.Clinic.Domain.Doctor.ValueObjects;

public sealed record FullName
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string? Title { get; init; }

    private FullName()
    {
    }

    private FullName(string firstName, string lastName, string? title)
    {
        FirstName = firstName;
        LastName = lastName;
        Title = title;
    }

    public static ErrorOr<FullName> Create(string firstName, string lastName, string? title = null)
    {
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(firstName))
        {
            errors.Add(Error.Validation(
                code: "FullName.InvalidFirstName",
                description: "First name cannot be null, empty, or whitespace.",
                metadata: new Dictionary<string, object> { ["firstName"] = firstName }));
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            errors.Add(Error.Validation(
                code: "FullName.InvalidLastName",
                description: "Last name cannot be null, empty, or whitespace.",
                metadata: new Dictionary<string, object> { ["lastName"] = lastName }));
        }

        if (errors.Any())
        {
            return errors;
        }

        return new FullName(firstName, lastName, title);
    }
}
