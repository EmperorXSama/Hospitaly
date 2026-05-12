using ErrorOr;
using Hospitaly.Modules.Clinic.Domain.Clinic.ValueObjects;

namespace Hospitaly.Modules.Clinic.Domain.Doctor.ValueObjects;

public sealed record ContactInfo
{
    public PhoneNumber? PhoneNumber { get; init; }
    public Email? Email { get; init; }
    public string? EmergencyContact { get; init; }

    private ContactInfo()
    {
    }

    private ContactInfo(PhoneNumber? phoneNumber, Email? email, string? emergencyContact)
    {
        PhoneNumber = phoneNumber;
        Email = email;
        EmergencyContact = emergencyContact;
    }

    public static ErrorOr<ContactInfo> Create(string? phoneNumber, string? email, string? emergencyContact = null)
    {
        var errors = new List<Error>();

        PhoneNumber? phone = null;
        if (!string.IsNullOrWhiteSpace(phoneNumber))
        {
            var phoneResult = PhoneNumber.Create(phoneNumber);
            if (phoneResult.IsError) errors.AddRange(phoneResult.Errors);
            else phone = phoneResult.Value;
        }

        Email? emailObj = null;
        if (!string.IsNullOrWhiteSpace(email))
        {
            var emailResult = Email.Create(email);
            if (emailResult.IsError) errors.AddRange(emailResult.Errors);
            else emailObj = emailResult.Value;
        }

        if (errors.Any())
        {
            return errors;
        }

        return new ContactInfo(phone, emailObj, emergencyContact);
    }
}
