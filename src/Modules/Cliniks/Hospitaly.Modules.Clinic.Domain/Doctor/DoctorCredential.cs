using ErrorOr;
using Hospitaly.Common.Domain;
using Hospitaly.Modules.Clinic.Domain.Doctor.Enums;
using Hospitaly.Modules.Clinic.Domain.Doctor.ValueObjects;

namespace Hospitaly.Modules.Clinic.Domain.Doctor;

public class DoctorCredential : Entity
{
    public Guid DoctorId { get; private set; }
    
    public CredentialType CredentialType { get; private set; }
    public string IssuingAuthority { get; private set; } = string.Empty;
    public string DocumentNumber { get; private set; } = string.Empty;
    public CredentialValidityPeriod ValidityPeriod { get; private set; }
    public CredentialStatus Status { get; private set; }
    
    public DateTime? VerifiedAt { get; private set; }
    public Guid? VerifiedBy { get; private set; }

    // EF Core
    private DoctorCredential()
    {
    }

    // Required by base Entity class
    private DoctorCredential(AuditInfo audit) : base(audit)
    {
    }

    private DoctorCredential(
        Guid doctorId,
        CredentialType credentialType,
        string issuingAuthority,
        string documentNumber,
        CredentialValidityPeriod validityPeriod,
        CredentialStatus status,
        AuditInfo audit) : base(audit)
    {
        DoctorId = doctorId;
        CredentialType = credentialType;
        IssuingAuthority = issuingAuthority;
        DocumentNumber = documentNumber;
        ValidityPeriod = validityPeriod;
        Status = status;
    }

    public static ErrorOr<DoctorCredential> Create(
        Guid doctorId,
        CredentialType credentialType,
        string issuingAuthority,
        string documentNumber,
        DateTimeOffset issueDate,
        DateTimeOffset expiryDate,
        Guid createdBy,
        DateTime createdOnUtc)
    {
        // Business rule: IssueDate cannot be in the future
        if (issueDate > DateTimeOffset.UtcNow)
        {
            return DoctorErrors.CredentialIssueDateCannotBeInTheFuture();
        }

        // Business rule: DocumentNumber must not be empty
        if (string.IsNullOrWhiteSpace(documentNumber))
        {
            return DoctorErrors.CredentialDocumentNumberIsRequired();
        }

        // Business rule: IssuingAuthority must not be empty
        if (string.IsNullOrWhiteSpace(issuingAuthority))
        {
            return DoctorErrors.CredentialIssuingAuthorityIsRequired();
        }

        // Create validity period (this validates ExpiryDate > IssueDate)
        var validityPeriod = CredentialValidityPeriod.Create(issueDate, expiryDate);
        if (validityPeriod.IsError)
        {
            return DoctorErrors.CredentialExpiryDateMustBeAfterIssueDate();
        }

        var audit = new AuditInfo(createdBy, createdOnUtc);

        var credential = new DoctorCredential(
            doctorId,
            credentialType,
            issuingAuthority,
            documentNumber,
            validityPeriod.Value,
            CredentialStatus.Active,
            audit);

        return credential;
    }

    public ErrorOr<Success> Verify(Guid verifiedBy, DateTime verifiedOnUtc)
    {
        var verifiedOnUtcOffset = new DateTimeOffset(verifiedOnUtc, TimeSpan.Zero);
        
        // Business rule: VerifiedAt cannot precede IssueDate
        if (verifiedOnUtcOffset < ValidityPeriod.IssueDate)
        {
            return DoctorErrors.CredentialVerificationDateCannotPrecedeIssueDate();
        }

        VerifiedAt = verifiedOnUtc;
        VerifiedBy = verifiedBy;

        return Result.Success;
    }

    public ErrorOr<Success> Revoke()
    {
        // Business rule: Revoked credentials can never be reactivated
        if (Status == CredentialStatus.Revoked)
        {
            return DoctorErrors.CredentialAlreadyRevoked();
        }

        Status = CredentialStatus.Revoked;

        return Result.Success;
    }

    public ErrorOr<Success> Suspend()
    {
        // Business rule: Cannot suspend an already revoked credential
        if (Status == CredentialStatus.Revoked)
        {
            return DoctorErrors.CredentialIsRevoked();
        }

        Status = CredentialStatus.Suspended;

        return Result.Success;
    }

    public ErrorOr<Success> Reactivate()
    {
        // Business rule: Revoked credentials can never be reactivated
        if (Status == CredentialStatus.Revoked)
        {
            return DoctorErrors.CredentialCannotBeReactivatedIfRevoked();
        }

        Status = CredentialStatus.Active;

        return Result.Success;
    }

    public bool IsValid(DateTime asOf)
    {
        var asOfOffset = new DateTimeOffset(asOf, TimeSpan.Zero);
        return Status == CredentialStatus.Active && !ValidityPeriod.IsExpired(asOfOffset) && VerifiedAt.HasValue;
    }
}



