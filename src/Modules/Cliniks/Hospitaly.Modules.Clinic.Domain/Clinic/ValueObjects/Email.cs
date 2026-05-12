using ErrorOr;
using System.Text.RegularExpressions;

namespace Hospitaly.Modules.Clinic.Domain.Clinic.ValueObjects;

public sealed partial record Email
{
    public string Value { get; init; }

    private Email()
    {
    }

    private Email(string value)
    {
        Value = value;
    }

    public static ErrorOr<Email> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Error.Validation(
                code: "Email.Invalid",
                description: "Email cannot be null, empty, or whitespace.",
                metadata: new Dictionary<string, object> { ["value"] = value });
        }

        if (!MyRegex().IsMatch(value))
        {
            return Error.Validation(
                code: "Email.InvalidFormat",
                description: "Email must be in a valid format.",
                metadata: new Dictionary<string, object> { ["value"] = value });
        }

        return new Email(value);
    }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex MyRegex();
}
