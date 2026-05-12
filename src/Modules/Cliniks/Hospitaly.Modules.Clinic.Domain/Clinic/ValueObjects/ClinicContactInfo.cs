using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hospitaly.Modules.Clinic.Domain.Clinic.ValueObjects;

public sealed record ClinicContactInfo
{
    public PhoneNumber? PhoneNumber { get; init; }
    public Email? Email { get; init; }
    public string? Website { get; init; }

    private ClinicContactInfo()
    {
    }

    private ClinicContactInfo(PhoneNumber? phoneNumber, Email? email, string? website)
    {
        PhoneNumber = phoneNumber;
        Email = email;
        Website = website;
    }

    public static ErrorOr<ClinicContactInfo> Create(string? phoneNumber, string? email, string? website)
    {
        var errors = new List<Error>();

        PhoneNumber? phone = null;
        if (!string.IsNullOrWhiteSpace(phoneNumber))
        {
            var phoneResult = PhoneNumber.Create(phoneNumber);
            if (phoneResult.IsError)
            {
                errors.AddRange(phoneResult.Errors);
            }
            else
            {
                phone = phoneResult.Value;
            }
        }

        Email? emailObj = null;
        if (!string.IsNullOrWhiteSpace(email))
        {
            var emailResult = Email.Create(email);
            if (emailResult.IsError)
            {
                errors.AddRange(emailResult.Errors);
            }
            else
            {
                emailObj = emailResult.Value;
            }
        }

        // At least one contact method must be provided
        if (phone is null && emailObj is null && string.IsNullOrWhiteSpace(website))
        {
            errors.Add(Error.Validation(
                code: "ClinicContactInfo.NoContactMethod",
                description: "At least one contact method (phone, email, or website) must be provided."));
        }

        // Basic website validation (optional)
        if (!string.IsNullOrWhiteSpace(website) &&
            !Uri.TryCreate(website, UriKind.Absolute, out _))
        {
            errors.Add(Error.Validation(
                code: "ClinicContactInfo.InvalidWebsite",
                description: "Website must be a valid URL.",
                metadata: new Dictionary<string, object> { ["website"] = website }));
        }

        if (errors.Any())
        {
            return errors;
        }

        return new ClinicContactInfo(phone, emailObj, website);
    }
}
