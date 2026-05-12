using ErrorOr;

namespace Hospitaly.Modules.Clinic.Domain.StaffMember;

public static class StaffMemberErrors
{
    public static Error IdentityIdRequired() =>
        Error.Validation("StaffMember.IdentityIdRequired", "Identity ID is required.");

    public static Error NameRequired() =>
        Error.Validation("StaffMember.NameRequired", "First and last name are required.");

    public static Error StaffRoleRequired() =>
        Error.Validation("StaffMember.StaffRoleRequired", "Staff role is required.");

    public static Error EmploymentInfoRequired() =>
        Error.Validation("StaffMember.EmploymentInfoRequired", "Employment info is required.");

    public static Error NotFound(Guid staffMemberId) =>
        Error.NotFound("StaffMember.NotFound", $"The staff member with identifier {staffMemberId} was not found.");

    public static Error InvalidEmail() =>
        Error.Validation("StaffMember.InvalidEmail", "The provided email is not valid.");
}
