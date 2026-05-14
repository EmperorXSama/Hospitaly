using ErrorOr;
using Hospitaly.Common.Domain;

namespace Hospitaly.Modules.Clinic.Domain.Clinic.Entities;

public class Department : Entity
{
    public string Name { get; private set; }
    public string Code { get; private set; }
    public bool IsActive { get; private set; }
    public Guid? ParentId { get; private set; }
    public Guid ClinicId { get; private set; }

    public Department? Parent { get; private set; }
    public ICollection<Department> Children { get; private set; } = [];
    public Clinic Clinic { get; private set; }

    private Department()
    {
    }

    private Department(string name, string code, bool isActive, Guid? parentId, Guid clinicId, AuditInfo audit)
        : base(audit,Guid.NewGuid())
    {
        Name = name;
        Code = code;
        IsActive = isActive;
        ParentId = parentId;
        ClinicId = clinicId;
    }

    public ErrorOr<Success> Update(string name, string code, Guid? parentId, Guid updatedById, DateTimeOffset updatedOn)
    {
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(name))
            errors.Add(Error.Validation("Department.InvalidName", "Department name cannot be empty."));

        if (string.IsNullOrWhiteSpace(code))
            errors.Add(Error.Validation("Department.InvalidCode", "Department code cannot be empty."));

        if (errors.Any())
            return errors;

        Name = name;
        Code = code;
        ParentId = parentId;
        SetUpdated(updatedById, updatedOn);
        return Result.Success;
    }

    public ErrorOr<Success> SetActiveState(bool isActive, Guid updatedById, DateTimeOffset updatedOn)
    {
        IsActive = isActive;
        SetUpdated(updatedById, updatedOn);
        return Result.Success;
    }

    public static ErrorOr<Department> Create(
        string name,
        string code,
        bool isActive,
        Guid clinicId,
        Guid? parentId = null,
        Guid? createdBy = null,
        DateTime? createdOnUtc = null)
    {
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(name))
        {
            errors.Add(Error.Validation(
                code: "Department.InvalidName",
                description: "Department name cannot be null, empty, or whitespace.",
                metadata: new Dictionary<string, object> { ["name"] = name }));
        }

        if (string.IsNullOrWhiteSpace(code))
        {
            errors.Add(Error.Validation(
                code: "Department.InvalidCode",
                description: "Department code cannot be null, empty, or whitespace.",
                metadata: new Dictionary<string, object> { ["code"] = code }));
        }

        if (clinicId == Guid.Empty)
        {
            errors.Add(Error.Validation(
                code: "Department.InvalidClinicId",
                description: "Clinic identifier cannot be empty."));
        }

        if (errors.Any())
        {
            return errors;
        }

        var audit = new AuditInfo(createdBy ?? Guid.Empty, createdOnUtc ?? DateTime.UtcNow);
        return new Department(name, code, isActive, parentId, clinicId, audit);
    }
}
