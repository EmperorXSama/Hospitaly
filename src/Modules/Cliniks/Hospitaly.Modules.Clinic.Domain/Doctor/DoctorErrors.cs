using ErrorOr;

namespace Hospitaly.Modules.Clinic.Domain.Doctor;

public static class DoctorErrors
{
    // DoctorCredential Errors
    public static Error CredentialExpiryDateMustBeAfterIssueDate() =>
        Error.Validation(
            "Doctor.Credential.ExpiryDateMustBeAfterIssueDate",
            "The credential expiry date must be after or equal to the issue date.");

    public static Error CredentialIssueDateCannotBeInTheFuture() =>
        Error.Validation(
            "Doctor.Credential.IssueDateCannotBeInTheFuture",
            "The credential issue date cannot be in the future.");

    public static Error CredentialDocumentNumberIsRequired() =>
        Error.Validation(
            "Doctor.Credential.DocumentNumberIsRequired",
            "The credential document number is required.");

    public static Error CredentialIssuingAuthorityIsRequired() =>
        Error.Validation(
            "Doctor.Credential.IssuingAuthorityIsRequired",
            "The credential issuing authority is required.");

    public static Error CredentialVerificationDateCannotPrecedeIssueDate() =>
        Error.Validation(
            "Doctor.Credential.VerificationDateCannotPrecedeIssueDate",
            "The verification date cannot precede the credential issue date.");

    public static Error CredentialAlreadyRevoked() =>
        Error.Conflict(
            "Doctor.Credential.AlreadyRevoked",
            "The credential has already been revoked and cannot be modified.");

    public static Error CredentialIsRevoked() =>
        Error.Conflict(
            "Doctor.Credential.IsRevoked",
            "The credential has been revoked and cannot be suspended.");

    public static Error CredentialCannotBeReactivatedIfRevoked() =>
        Error.Conflict(
            "Doctor.Credential.CannotBeReactivatedIfRevoked",
            "Revoked credentials cannot be reactivated. A new credential must be submitted.");

    public static Error DoctorNotFound(Guid doctorId) =>
        Error.NotFound(
            "Doctor.NotFound",
            $"The doctor with identifier {doctorId} was not found.");

    public static Error NoRequiredCredentials(Guid doctorId) =>
        Error.Validation(
            "Doctor.NoRequiredCredentials",
            $"The doctor with identifier {doctorId} does not have all required credentials.");

    public static Error DoctorCannotBeActivatedWithExpiredCredentials(Guid doctorId) =>
        Error.Conflict(
            "Doctor.CannotBeActivatedWithExpiredCredentials",
            $"The doctor with identifier {doctorId} cannot be activated because one or more credentials are expired or not verified.");

    // ClinicAffiliation Errors
    public static Error AffiliationAlreadyTerminated() =>
        Error.Conflict(
            "Doctor.Affiliation.AlreadyTerminated",
            "The affiliation has already been terminated and cannot be modified.");
}

