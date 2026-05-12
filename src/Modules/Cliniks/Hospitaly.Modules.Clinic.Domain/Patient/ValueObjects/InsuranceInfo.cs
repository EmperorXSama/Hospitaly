using ErrorOr;
using Hospitaly.Common.Domain.Common.ValueObjects;

namespace Hospitaly.Modules.Clinic.Domain.Patient.ValueObjects;

public sealed record InsuranceInfo
{
    public DateTimeRange Value { get; init; }
    public string InsurerName { get; init; }
    public string PolicyNumber { get; init; }
    public string? GroupNumber { get; init; }

    public DateTimeOffset ValidFrom => Value.Start;
    public DateTimeOffset? ValidUntil => Value.End;
    public bool HasExpiry => Value.IsClosed;
    public bool IsPerpetual => Value.IsOpenEnded;

    private InsuranceInfo()
    {
    }

    private InsuranceInfo(DateTimeRange value, string insurerName, string policyNumber, string? groupNumber)
    {
        Value = value;
        InsurerName = insurerName;
        PolicyNumber = policyNumber;
        GroupNumber = groupNumber;
    }

    public static ErrorOr<InsuranceInfo> Create(
        string insurerName,
        string policyNumber,
        string? groupNumber,
        DateTimeOffset validFrom,
        DateTimeOffset? validUntil)
    {
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(insurerName))
        {
            errors.Add(Error.Validation(
                code: "InsuranceInfo.InvalidInsurerName",
                description: "Insurer name cannot be null, empty, or whitespace.",
                metadata: new Dictionary<string, object> { ["insurerName"] = insurerName }));
        }

        if (string.IsNullOrWhiteSpace(policyNumber))
        {
            errors.Add(Error.Validation(
                code: "InsuranceInfo.PolicyNumberRequired",
                description: "Policy number cannot be null, empty, or whitespace.",
                metadata: new Dictionary<string, object> { ["policyNumber"] = policyNumber }));
        }

        var dateTimeRange = DateTimeRange.Create(validFrom, validUntil);
        if (dateTimeRange.IsError)
        {
            errors.AddRange(dateTimeRange.Errors);
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return new InsuranceInfo(dateTimeRange.Value, insurerName, policyNumber, groupNumber);
    }

    public bool IsActive(DateTimeOffset asOf) => Value.Contains(asOf);
}
