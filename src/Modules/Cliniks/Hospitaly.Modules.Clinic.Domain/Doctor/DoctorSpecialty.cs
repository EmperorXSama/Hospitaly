using ErrorOr;

namespace Hospitaly.Modules.Clinic.Domain.Doctor;

public class DoctorSpecialty : IEquatable<DoctorSpecialty>
{
    public Guid DoctorId { get; private set; }
    public Guid SpecialtyId { get; private set; }

    public bool IsPrimary { get; private set; }
    public string? CertificationNumber { get; private set; }
    public DateTime CertifiedAt { get; private set; }

    public Doctor Doctor { get; private set; }
    public Specialty.Specialty Specialty { get; private set; }

    private DoctorSpecialty()
    {
    }

    private DoctorSpecialty(
        Guid doctorId,
        Guid specialtyId,
        bool isPrimary,
        string? certificationNumber,
        DateTime certifiedAt)
    {
        DoctorId = doctorId;
        SpecialtyId = specialtyId;
        IsPrimary = isPrimary;
        CertificationNumber = certificationNumber;
        CertifiedAt = certifiedAt;
    }

    public static ErrorOr<DoctorSpecialty> Create(
        Guid doctorId,
        Guid specialtyId,
        bool isPrimary,
        string? certificationNumber,
        DateTime certifiedAt)
    {
        var errors = new List<Error>();

        if (doctorId == Guid.Empty)
        {
            errors.Add(Error.Validation(
                code: "DoctorSpecialty.InvalidDoctorId",
                description: "Doctor identifier cannot be empty."));
        }

        if (specialtyId == Guid.Empty)
        {
            errors.Add(Error.Validation(
                code: "DoctorSpecialty.InvalidSpecialtyId",
                description: "Specialty identifier cannot be empty."));
        }

        if (string.IsNullOrWhiteSpace(certificationNumber))
        {
            errors.Add(Error.Validation(
                code: "DoctorSpecialty.InvalidCertificationNumber",
                description: "Certification number cannot be null, empty, or whitespace."));
        }

        if (errors.Any())
        {
            return errors;
        }

        return new DoctorSpecialty(doctorId, specialtyId, isPrimary, certificationNumber, certifiedAt);
    }

    public bool Equals(DoctorSpecialty? other)
    {
        if (other is null) return false;
        return DoctorId == other.DoctorId && SpecialtyId == other.SpecialtyId;
    }

    public override bool Equals(object? obj) => Equals(obj as DoctorSpecialty);

    public override int GetHashCode() => HashCode.Combine(DoctorId, SpecialtyId);
}