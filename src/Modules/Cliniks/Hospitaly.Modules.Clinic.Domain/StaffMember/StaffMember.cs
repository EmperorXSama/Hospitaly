using ErrorOr;
using Hospitaly.Common.Domain;
using Hospitaly.Modules.Clinic.Domain.StaffMember.ValueObjects;

namespace Hospitaly.Modules.Clinic.Domain.StaffMember;

public class StaffMember : AggregateRoot
{
    public string IdentityId { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string? Phone { get; private set; }
    public string? Email { get; private set; }
    public StaffRole Role { get; private set; } = null!;
    public EmploymentInfo Employment { get; private set; } = null!;

    private StaffMember()
    {
    }

    private StaffMember(AuditInfo audit) : base(audit)
    {
    }

    public static ErrorOr<StaffMember> Create(
        string identityId,
        string firstName,
        string lastName,
        StaffRole role,
        EmploymentInfo employment,
        string? phone,
        string? email,
        Guid createdBy,
        DateTime createdOnUtc)
    {
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(identityId))
        {
            errors.Add(StaffMemberErrors.IdentityIdRequired());
        }

        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
        {
            errors.Add(StaffMemberErrors.NameRequired());
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        var audit = new AuditInfo(createdBy, createdOnUtc);

        var staffMember = new StaffMember(audit)
        {
            IdentityId = identityId,
            FirstName = firstName,
            LastName = lastName,
            Phone = phone,
            Email = email,
            Role = role,
            Employment = employment,
        };

        staffMember.RaiseDomainEvent(new StaffMemberCreatedDomainEvent(
            staffMember.Id, identityId, firstName, lastName, role, employment));

        return staffMember;
    }

    public ErrorOr<Success> ChangeRole(StaffRole newRole, Guid updatedBy, DateTime updatedOnUtc)
    {
        var oldRole = Role;
        Role = newRole;

        RaiseDomainEvent(new StaffMemberRoleChangedDomainEvent(Id, oldRole, newRole));
        SetUpdated(updatedBy, updatedOnUtc);

        return Result.Success;
    }

    public ErrorOr<Success> UpdateEmployment(EmploymentInfo newEmployment, Guid updatedBy, DateTime updatedOnUtc)
    {
        var oldEmployment = Employment;
        Employment = newEmployment;

        RaiseDomainEvent(new StaffMemberEmploymentUpdatedDomainEvent(Id, oldEmployment, newEmployment));
        SetUpdated(updatedBy, updatedOnUtc);

        return Result.Success;
    }

    public ErrorOr<Success> UpdateContact(string? phone, string? email, Guid updatedBy, DateTime updatedOnUtc)
    {
        var oldPhone = Phone;
        var oldEmail = Email;
        Phone = phone;
        Email = email;

        RaiseDomainEvent(new StaffMemberContactUpdatedDomainEvent(Id, oldPhone, phone, oldEmail, email));
        SetUpdated(updatedBy, updatedOnUtc);

        return Result.Success;
    }
}
