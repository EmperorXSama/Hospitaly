namespace Hospitaly.Modules.Clinic.Application.Clinic.Queries.GetClinicDepartments;

public sealed record DepartmentTreeNodeResponse(
    Guid Id,
    string Name,
    string Code,
    bool IsActive,
    Guid? ParentId,
    List<DepartmentTreeNodeResponse> Children);
