using ErrorOr;
using Hospitaly.Modules.Clinic.Domain.StaffMember.Enums;

namespace Hospitaly.Modules.Clinic.Domain.StaffMember.ValueObjects;

public sealed record StaffRole
{
    public StaffRoleEnum Role { get; }
    public string Department { get; }

    private StaffRole()
    {
    }

    private StaffRole(StaffRoleEnum role, string department)
    {
        Role = role;
        Department = department;
    }

    public static ErrorOr<StaffRole> Create(StaffRoleEnum role, string department)
    {
        var errors = new List<Error>();

        if (role == StaffRoleEnum.None)
        {
            errors.Add(Error.Validation(
                "StaffRole.Invalid",
                "Staff role cannot be None."));
        }

        if (string.IsNullOrWhiteSpace(department))
        {
            errors.Add(Error.Validation(
                "StaffRole.DepartmentRequired",
                "Department is required."));
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return new StaffRole(role, department);
    }

    public override string ToString() => $"{Role} ({Department})";
}
