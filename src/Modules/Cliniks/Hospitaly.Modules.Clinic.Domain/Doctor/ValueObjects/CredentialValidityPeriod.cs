using ErrorOr;
using Hospitaly.Common.Domain.Common.ValueObjects;

namespace Hospitaly.Modules.Clinic.Domain.Doctor.ValueObjects;

/// <summary>
/// Value object representing the validity period of a doctor credential.
/// Encapsulates the issue date and optional expiry date with validation logic.
/// </summary>
public sealed record CredentialValidityPeriod
{
    public DateTimeRange Value { get; init; }

    private CredentialValidityPeriod()
    {
    }

    private CredentialValidityPeriod(DateTimeRange value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new credential validity period.
    /// </summary>
    /// <param name="issueDate">When the credential was issued</param>
    /// <param name="expiryDate">When the credential expires (null for perpetual credentials)</param>
    /// <returns>A CredentialValidityPeriod or validation error</returns>
    public static ErrorOr<CredentialValidityPeriod> Create(DateTimeOffset issueDate, DateTimeOffset expiryDate)
    {
        var dateTimeRange = DateTimeRange.Create(issueDate, expiryDate);

        if (dateTimeRange.IsError)
        {
            return dateTimeRange.Errors;
        }

        return new CredentialValidityPeriod(dateTimeRange.Value);
    }

    /// <summary>
    /// Determines if the credential has expired as of the given date.
    /// </summary>
    public bool IsExpired(DateTimeOffset asOf) => !Value.Contains(asOf) && Value.IsClosed;

    /// <summary>
    /// Gets the issue date of the credential.
    /// </summary>
    public DateTimeOffset IssueDate => Value.Start;

    /// <summary>
    /// Gets the expiry date of the credential, if it expires.
    /// </summary>
    public DateTimeOffset? ExpiryDate => Value.End;

    /// <summary>
    /// Determines if the credential has an expiry date.
    /// </summary>
    public bool HasExpiry => Value.IsClosed;

    /// <summary>
    /// Determines if the credential is perpetual (no expiry date).
    /// </summary>
    public bool IsPerpetual => Value.IsOpenEnded;
}

