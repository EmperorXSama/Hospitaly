using ErrorOr;
using Hospitaly.Modules.Clinic.Domain.Clinic.ValueObjects;

namespace Hospitaly.Modules.Clinic.Domain.Patient.ValueObjects;

public sealed record ContactInfo
{
    public PhoneNumber? PhoneNumber { get; init; }
    public Email? Email { get; init; }
    public Address? Address { get; init; }

    private ContactInfo()
    {
    }

    private ContactInfo(PhoneNumber? phoneNumber, Email? email, Address? address)
    {
        PhoneNumber = phoneNumber;
        Email = email;
        Address = address;
    }

    public static ErrorOr<ContactInfo> Create(string? phoneNumber, string? email, Address? address)
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

        if (phone is null && emailObj is null)
        {
            errors.Add(Error.Validation(
                code: "ContactInfo.PhoneOrEmailRequired",
                description: "At least one of phone number or email must be provided."));
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return new ContactInfo(phone, emailObj, address);
    }
}
