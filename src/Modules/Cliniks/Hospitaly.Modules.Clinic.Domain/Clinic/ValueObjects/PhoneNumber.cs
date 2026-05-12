using ErrorOr;
using System.Text.RegularExpressions;

namespace Hospitaly.Modules.Clinic.Domain.Clinic.ValueObjects;

public sealed record PhoneNumber
{
    public string Value { get; init; }
    private PhoneNumber(){}
    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static ErrorOr<PhoneNumber> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Error.Validation(
                code: "PhoneNumber.Invalid",
                description: "Phone number cannot be null, empty, or whitespace.",
                metadata: new Dictionary<string, object> { ["value"] = value });
        }

        // Moroccan phone number regex: starts with +212 or 0, followed by 5-7 and 8 digits
        if (!Regex.IsMatch(value, @"^(\+212|0)[5-7]\d{8}$"))
        {
            return Error.Validation(
                code: "PhoneNumber.InvalidMoroccanFormat",
                description: "Phone number must be a valid Moroccan format (e.g., +2126xxxxxxxx or 06xxxxxxxx).",
                metadata: new Dictionary<string, object> { ["value"] = value });
        }

        return new PhoneNumber(value);
    }
}
