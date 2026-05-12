using ErrorOr;
using Hospitaly.Common.Domain;

namespace Hospitaly.Modules.Clinic.Domain.Doctor;

public class Doctor : AggregateRoot
{
    private readonly List<DoctorCredential> _credentials = [];
    private readonly List<DoctorSpecialty> _specialties = [];
    private readonly List<ClinicAffiliation> _affiliations = [];

    public IReadOnlyCollection<DoctorCredential> Credentials => _credentials.AsReadOnly();
    public IReadOnlyCollection<DoctorSpecialty> Specialties => _specialties.AsReadOnly();
    public IReadOnlyCollection<ClinicAffiliation> Affiliations => _affiliations.AsReadOnly();

    // EF Core
    private Doctor()
    {
    }

    // Required by base AggregateRoot class
    protected Doctor(AuditInfo audit) : base(audit)
    {
    }

    public static ErrorOr<Doctor> Create(
        Guid createdBy,
        DateTime createdOnUtc)
    {
        
        var audit = new AuditInfo(createdBy, createdOnUtc);
        var doctor = new Doctor(audit);
        return doctor;
    }
}

