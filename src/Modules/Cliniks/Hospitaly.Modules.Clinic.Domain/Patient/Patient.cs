using ErrorOr;
using Hospitaly.Common.Domain;
using Hospitaly.Modules.Clinic.Domain.Clinic.ValueObjects;
using Hospitaly.Modules.Clinic.Domain.Patient.Enums;
using Hospitaly.Modules.Clinic.Domain.Patient.ValueObjects;

namespace Hospitaly.Modules.Clinic.Domain.Patient;

public class Patient : AggregateRoot
{
    public PatientIdentity Identity { get; private set; }
    public ContactInfo Contact { get; private set; }
    public InsuranceInfo? Insurance { get; private set; }
    public PatientType PatientType { get; private set; }

    private Patient()
    {
    }

    protected Patient(AuditInfo audit) : base(audit,Guid.NewGuid())
    {
    }

    public static ErrorOr<Patient> Create(
        string firstName,
        string lastName,
        DateOnly dateOfBirth,
        Gender gender,
        string nationalId,
        string? phoneNumber,
        string? email,
        string? addressStreet,
        string? addressCity,
        string? addressRegion,
        string? addressPostalCode,
        string? addressCountry,
        VisitType visitType,
        DateTime? registrationDate,
        string? insurerName,
        string? policyNumber,
        string? groupNumber,
        DateTimeOffset? validFrom,
        DateTimeOffset? validUntil,
        Guid createdBy,
        DateTime createdOnUtc)
    {
        var errors = new List<Error>();

        var identity = PatientIdentity.Create(firstName, lastName, dateOfBirth, gender, nationalId);
        if (identity.IsError) errors.AddRange(identity.Errors);

        Address? address = null;
        if (!string.IsNullOrWhiteSpace(addressStreet) || !string.IsNullOrWhiteSpace(addressCity))
        {
            var addressResult = Address.Create(
                addressStreet ?? string.Empty,
                addressCity ?? string.Empty,
                addressRegion,
                addressPostalCode,
                addressCountry ?? string.Empty);

            if (addressResult.IsError) errors.AddRange(addressResult.Errors);
            else address = addressResult.Value;
        }

        var contact = ContactInfo.Create(phoneNumber, email, address);
        if (contact.IsError) errors.AddRange(contact.Errors);

        var patientType = PatientType.Create(visitType, registrationDate);
        if (patientType.IsError) errors.AddRange(patientType.Errors);

        InsuranceInfo? insurance = null;
        if (!string.IsNullOrWhiteSpace(insurerName) || !string.IsNullOrWhiteSpace(policyNumber))
        {
            var insuranceResult = InsuranceInfo.Create(
                insurerName ?? string.Empty,
                policyNumber ?? string.Empty,
                groupNumber,
                validFrom ?? DateTimeOffset.UtcNow,
                validUntil);

            if (insuranceResult.IsError) errors.AddRange(insuranceResult.Errors);
            else insurance = insuranceResult.Value;
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        var audit = new AuditInfo(createdBy, createdOnUtc);

        return new Patient(audit)
        {
            Identity = identity.Value,
            Contact = contact.Value,
            Insurance = insurance,
            PatientType = patientType.Value,
        };
    }

    public ErrorOr<Success> UpdateContact(
        string? phoneNumber,
        string? email,
        string? addressStreet,
        string? addressCity,
        string? addressRegion,
        string? addressPostalCode,
        string? addressCountry,
        Guid updatedBy,
        DateTime updatedOnUtc)
    {
        Address? address = null;
        if (!string.IsNullOrWhiteSpace(addressStreet) || !string.IsNullOrWhiteSpace(addressCity))
        {
            var addressResult = Address.Create(
                addressStreet ?? string.Empty,
                addressCity ?? string.Empty,
                addressRegion,
                addressPostalCode,
                addressCountry ?? string.Empty);

            if (addressResult.IsError) return addressResult.Errors;
            address = addressResult.Value;
        }

        var contact = ContactInfo.Create(phoneNumber, email, address);
        if (contact.IsError)
        {
            return contact.Errors;
        }

        Contact = contact.Value;
        SetUpdated(updatedBy, updatedOnUtc);
        return Result.Success;
    }

    public ErrorOr<Success> SetInsurance(
        string insurerName,
        string policyNumber,
        string? groupNumber,
        DateTimeOffset validFrom,
        DateTimeOffset? validUntil,
        Guid updatedBy,
        DateTime updatedOnUtc)
    {
        var insurance = InsuranceInfo.Create(insurerName, policyNumber, groupNumber, validFrom, validUntil);
        if (insurance.IsError)
        {
            return insurance.Errors;
        }

        Insurance = insurance.Value;
        SetUpdated(updatedBy, updatedOnUtc);
        return Result.Success;
    }

    public ErrorOr<Success> RemoveInsurance(Guid updatedBy, DateTime updatedOnUtc)
    {
        Insurance = null;
        SetUpdated(updatedBy, updatedOnUtc);
        return Result.Success;
    }

    public ErrorOr<Success> Register(DateTime registrationDate, Guid updatedBy, DateTime updatedOnUtc)
    {
        if (PatientType.IsRegistered)
        {
            return PatientErrors.AlreadyRegistered();
        }

        var patientType = PatientType.Create(VisitType.Registered, registrationDate);
        if (patientType.IsError)
        {
            return patientType.Errors;
        }

        PatientType = patientType.Value;
        SetUpdated(updatedBy, updatedOnUtc);
        return Result.Success;
    }
}
