using ErrorOr;

namespace Hospitaly.Modules.Clinic.Domain.Clinic.Entities;

public class ClinicSpecialty : IEquatable<ClinicSpecialty>
{
    public Guid ClinicId { get; private set; }
    public Guid SpecialtyId { get; private set; }
    public bool IsActive { get; private set; }
    public decimal? ConsultationFee { get; private set; }

    public Clinic Clinic { get; private set; }
    public Specialty.Specialty Specialty { get; private set; }

    private ClinicSpecialty()
    {
    }

    private ClinicSpecialty(
        Guid clinicId,
        Guid specialtyId,
        bool isActive,
        decimal? consultationFee)
    {
        ClinicId = clinicId;
        SpecialtyId = specialtyId;
        IsActive = isActive;
        ConsultationFee = consultationFee;
    }

    public static ErrorOr<ClinicSpecialty> Create(
        Guid clinicId,
        Guid specialtyId,
        bool isActive,
        decimal? consultationFee = null)
    {
        var errors = new List<Error>();

        if (clinicId == Guid.Empty)
        {
            errors.Add(Error.Validation(
                code: "ClinicSpecialty.InvalidClinicId",
                description: "Clinic identifier cannot be empty."));
        }

        if (specialtyId == Guid.Empty)
        {
            errors.Add(Error.Validation(
                code: "ClinicSpecialty.InvalidSpecialtyId",
                description: "Specialty identifier cannot be empty."));
        }

        if (consultationFee < 0)
        {
            errors.Add(Error.Validation(
                code: "ClinicSpecialty.InvalidConsultationFee",
                description: "Consultation fee cannot be negative.",
                metadata: new Dictionary<string, object> { ["consultationFee"] = consultationFee }));
        }

        if (errors.Any())
        {
            return errors;
        }

        return new ClinicSpecialty(clinicId, specialtyId, isActive, consultationFee);
    }

    public bool Equals(ClinicSpecialty? other)
    {
        if (other is null) return false;
        return ClinicId == other.ClinicId && SpecialtyId == other.SpecialtyId;
    }

    public override bool Equals(object? obj) => Equals(obj as ClinicSpecialty);

    public override int GetHashCode() => HashCode.Combine(ClinicId, SpecialtyId);
}
