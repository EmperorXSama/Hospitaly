using Hospitaly.Common.Domain;
using Hospitaly.Modules.Clinic.Domain.StaffMember.ValueObjects;

namespace Hospitaly.Modules.Clinic.Domain.StaffMember;

public sealed class StaffMemberCreatedDomainEvent : DomainEvent
{
    public Guid StaffMemberId { get; }
    public string IdentityId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public StaffRole Role { get; }
    public EmploymentInfo Employment { get; }

    public StaffMemberCreatedDomainEvent(
        Guid staffMemberId,
        string identityId,
        string firstName,
        string lastName,
        StaffRole role,
        EmploymentInfo employment)
    {
        StaffMemberId = staffMemberId;
        IdentityId = identityId;
        FirstName = firstName;
        LastName = lastName;
        Role = role;
        Employment = employment;
    }
}

public sealed class StaffMemberRoleChangedDomainEvent : DomainEvent
{
    public Guid StaffMemberId { get; }
    public StaffRole OldRole { get; }
    public StaffRole NewRole { get; }

    public StaffMemberRoleChangedDomainEvent(Guid staffMemberId, StaffRole oldRole, StaffRole newRole)
    {
        StaffMemberId = staffMemberId;
        OldRole = oldRole;
        NewRole = newRole;
    }
}

public sealed class StaffMemberEmploymentUpdatedDomainEvent : DomainEvent
{
    public Guid StaffMemberId { get; }
    public EmploymentInfo OldEmployment { get; }
    public EmploymentInfo NewEmployment { get; }

    public StaffMemberEmploymentUpdatedDomainEvent(
        Guid staffMemberId,
        EmploymentInfo oldEmployment,
        EmploymentInfo newEmployment)
    {
        StaffMemberId = staffMemberId;
        OldEmployment = oldEmployment;
        NewEmployment = newEmployment;
    }
}

public sealed class StaffMemberContactUpdatedDomainEvent : DomainEvent
{
    public Guid StaffMemberId { get; }
    public string? OldPhone { get; }
    public string? NewPhone { get; }
    public string? OldEmail { get; }
    public string? NewEmail { get; }

    public StaffMemberContactUpdatedDomainEvent(
        Guid staffMemberId,
        string? oldPhone,
        string? newPhone,
        string? oldEmail,
        string? newEmail)
    {
        StaffMemberId = staffMemberId;
        OldPhone = oldPhone;
        NewPhone = newPhone;
        OldEmail = oldEmail;
        NewEmail = newEmail;
    }
}
