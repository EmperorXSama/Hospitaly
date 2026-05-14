using ErrorOr;
using Hospitaly.Common.Domain;
using Hospitaly.Modules.Clinic.Domain.Doctor.Enums;
using Hospitaly.Modules.Clinic.Domain.Doctor.ValueObjects;

namespace Hospitaly.Modules.Clinic.Domain.Doctor;

public class ClinicAffiliation : Entity
{
    private readonly List<Privilege> _grantedPrivileges = [];

    public Guid ClinicId { get; private set; }
    public Guid DoctorId { get; private set; }
    public AffiliationStatus Status { get; private set; }
    public DateTime JoinedDate { get; private set; }
    public DateTime? TerminatedDate { get; private set; }
    public Guid? DepartmentId { get; private set; }
    public IReadOnlyCollection<Privilege> GrantedPrivileges => _grantedPrivileges.AsReadOnly();

    public Doctor Doctor { get; private set; }

    private ClinicAffiliation()
    {
    }

    private ClinicAffiliation(
        Guid clinicId,
        Guid doctorId,
        AffiliationStatus status,
        DateTime joinedDate,
        Guid? departmentId,
        List<Privilege> grantedPrivileges,
        AuditInfo audit) : base(audit,Guid.NewGuid())
    {
        ClinicId = clinicId;
        DoctorId = doctorId;
        Status = status;
        JoinedDate = joinedDate;
        DepartmentId = departmentId;
        _grantedPrivileges = grantedPrivileges;
    }

    public static ErrorOr<ClinicAffiliation> Create(
        Guid clinicId,
        Guid doctorId,
        DateTime joinedDate,
        Guid? departmentId = null,
        List<Privilege>? grantedPrivileges = null,
        Guid? createdBy = null,
        DateTime? createdOnUtc = null)
    {
        var errors = new List<Error>();

        if (clinicId == Guid.Empty)
        {
            errors.Add(Error.Validation(
                code: "ClinicAffiliation.InvalidClinicId",
                description: "Clinic identifier cannot be empty."));
        }

        if (doctorId == Guid.Empty)
        {
            errors.Add(Error.Validation(
                code: "ClinicAffiliation.InvalidDoctorId",
                description: "Doctor identifier cannot be empty."));
        }

        if (joinedDate > DateTime.UtcNow)
        {
            errors.Add(Error.Validation(
                code: "ClinicAffiliation.JoinedDateInFuture",
                description: "Joined date cannot be in the future.",
                metadata: new Dictionary<string, object> { ["joinedDate"] = joinedDate }));
        }

        if (errors.Any())
        {
            return errors;
        }

        var audit = new AuditInfo(createdBy ?? Guid.Empty, createdOnUtc ?? DateTime.UtcNow);
        var privileges = grantedPrivileges ?? [];
        return new ClinicAffiliation(clinicId, doctorId, AffiliationStatus.Pending, joinedDate, departmentId, privileges, audit);
    }

    public ErrorOr<Success> Activate()
    {
        if (Status == AffiliationStatus.Terminated)
        {
            return DoctorErrors.AffiliationAlreadyTerminated();
        }

        Status = AffiliationStatus.Active;
        return Result.Success;
    }

    public ErrorOr<Success> Suspend()
    {
        if (Status == AffiliationStatus.Terminated)
        {
            return DoctorErrors.AffiliationAlreadyTerminated();
        }

        Status = AffiliationStatus.Suspended;
        return Result.Success;
    }

    public ErrorOr<Success> Terminate(DateTime terminatedDate)
    {
        if (Status == AffiliationStatus.Terminated)
        {
            return DoctorErrors.AffiliationAlreadyTerminated();
        }

        Status = AffiliationStatus.Terminated;
        TerminatedDate = terminatedDate;
        return Result.Success;
    }
}