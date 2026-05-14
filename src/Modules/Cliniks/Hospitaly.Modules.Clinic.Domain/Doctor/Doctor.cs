using ErrorOr;
using Hospitaly.Common.Domain;
using Hospitaly.Modules.Clinic.Domain.Doctor.Enums;

namespace Hospitaly.Modules.Clinic.Domain.Doctor;

public class Doctor : AggregateRoot
{
    private readonly List<DoctorCredential> _credentials = [];
    private readonly List<DoctorSpecialty> _specialties = [];
    private readonly List<ClinicAffiliation> _affiliations = [];
    
    public IReadOnlyCollection<DoctorCredential> Credentials => _credentials.AsReadOnly();
    public IReadOnlyCollection<DoctorSpecialty> Specialties => _specialties.AsReadOnly();
    public IReadOnlyCollection<ClinicAffiliation> Affiliations => _affiliations.AsReadOnly();

    public string? Title { get; private set; }
    public string? Bio { get; private set; }
    public string? AvatarUrl { get; private set; }
    public DoctorStatus Status { get; private set; }

    // EF Core
    private Doctor()
    {
    }

    // Required by base AggregateRoot class
    protected Doctor(AuditInfo audit) : base(audit, Guid.NewGuid())
    {
        Status = DoctorStatus.Pending;
    }

    public static ErrorOr<Doctor> Create(
        Guid createdBy,
        DateTime createdOnUtc)
    {
        var audit = new AuditInfo(createdBy, createdOnUtc);
        var doctor = new Doctor(audit);
        return doctor;
    }

    public ErrorOr<Success> UpdateProfile(
        string? title,
        string? bio,
        string? avatarUrl,
        Guid updatedBy,
        DateTimeOffset updatedOnUtc)
    {
        Title = title;
        Bio = bio;
        AvatarUrl = avatarUrl;
        SetUpdated(updatedBy, updatedOnUtc);
        return Result.Success;
    }

    public ErrorOr<Success> UpdateAvatar(
        string? avatarUrl,
        Guid updatedBy,
        DateTimeOffset updatedOnUtc)
    {
        AvatarUrl = avatarUrl;
        SetUpdated(updatedBy, updatedOnUtc);
        return Result.Success;
    }

    public void AddCredential(DoctorCredential credential)
    {
        _credentials.Add(credential);
    }

    public ErrorOr<Success> VerifyCredential(
        Guid credentialId,
        Guid verifiedBy,
        DateTime verifiedOnUtc)
    {
        var credential = _credentials.FirstOrDefault(c => c.Id == credentialId);
        if (credential is null)
            return DoctorErrors.CredentialNotFound(credentialId);

        return credential.Verify(verifiedBy, verifiedOnUtc);
    }

    public ErrorOr<Success> RevokeCredential(Guid credentialId)
    {
        var credential = _credentials.FirstOrDefault(c => c.Id == credentialId);
        if (credential is null)
            return DoctorErrors.CredentialNotFound(credentialId);

        return credential.Revoke();
    }

    public ErrorOr<Success> SuspendCredential(Guid credentialId)
    {
        var credential = _credentials.FirstOrDefault(c => c.Id == credentialId);
        if (credential is null)
            return DoctorErrors.CredentialNotFound(credentialId);

        return credential.Suspend();
    }

    public ErrorOr<Success> ReactivateCredential(Guid credentialId)
    {
        var credential = _credentials.FirstOrDefault(c => c.Id == credentialId);
        if (credential is null)
            return DoctorErrors.CredentialNotFound(credentialId);

        return credential.Reactivate();
    }

    public ErrorOr<Success> Activate(Guid updatedBy, DateTimeOffset updatedOnUtc)
    {
        if (Status == DoctorStatus.Active)
            return DoctorErrors.DoctorAlreadyActive(Id);

        if (_credentials.Count == 0)
            return DoctorErrors.NoRequiredCredentials(Id);

        var now = updatedOnUtc.DateTime;
        if (_credentials.Any(c => !c.IsValid(now)))
            return DoctorErrors.DoctorCannotBeActivatedWithExpiredCredentials(Id);

        Status = DoctorStatus.Active;
        SetUpdated(updatedBy, updatedOnUtc);
        return Result.Success;
    }

    public ErrorOr<Success> Deactivate(Guid updatedBy, DateTimeOffset updatedOnUtc)
    {
        if (Status == DoctorStatus.Inactive)
            return DoctorErrors.DoctorAlreadyInactive(Id);

        Status = DoctorStatus.Inactive;
        SetUpdated(updatedBy, updatedOnUtc);
        return Result.Success;
    }

    public ErrorOr<Success> AddSpecialties(List<DoctorSpecialty> specialties)
    {
        foreach (var specialty in specialties)
        {
            if (_specialties.Any(s => s.SpecialtyId == specialty.SpecialtyId))
                return DoctorErrors.DuplicateSpecialty(specialty.SpecialtyId);

            _specialties.Add(specialty);
        }
        return Result.Success;
    }

    public ErrorOr<Success> RemoveSpecialty(Guid specialtyId)
    {
        var specialty = _specialties.FirstOrDefault(s => s.SpecialtyId == specialtyId);
        if (specialty is null)
            return DoctorErrors.SpecialtyNotFound(specialtyId);

        _specialties.Remove(specialty);
        return Result.Success;
    }

    public ErrorOr<Success> SetPrimarySpecialty(Guid specialtyId)
    {
        var specialty = _specialties.FirstOrDefault(s => s.SpecialtyId == specialtyId);
        if (specialty is null)
            return DoctorErrors.SpecialtyNotFound(specialtyId);

        foreach (var s in _specialties.Where(s => s.IsPrimary))
            s.SetPrimary(false);

        specialty.SetPrimary(true);
        return Result.Success;
    }

    public ErrorOr<Success> AddAffiliation(ClinicAffiliation affiliation)
    {
        if (_affiliations.Any(a => a.ClinicId == affiliation.ClinicId))
            return DoctorErrors.AlreadyAffiliatedWithClinic(affiliation.ClinicId);

        _affiliations.Add(affiliation);
        return Result.Success;
    }

    public ErrorOr<Success> ActivateAffiliation(Guid clinicId)
    {
        var affiliation = _affiliations.FirstOrDefault(a => a.ClinicId == clinicId);
        if (affiliation is null)
            return DoctorErrors.AffiliationNotFound(clinicId);

        return affiliation.Activate();
    }

    public ErrorOr<Success> SuspendAffiliation(Guid clinicId)
    {
        var affiliation = _affiliations.FirstOrDefault(a => a.ClinicId == clinicId);
        if (affiliation is null)
            return DoctorErrors.AffiliationNotFound(clinicId);

        return affiliation.Suspend();
    }
}

