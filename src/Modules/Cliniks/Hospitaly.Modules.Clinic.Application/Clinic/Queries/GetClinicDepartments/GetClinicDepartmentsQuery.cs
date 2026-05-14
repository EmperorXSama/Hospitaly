using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Queries.GetClinicDepartments;

public sealed record GetClinicDepartmentsQuery(Guid ClinicId) : IQuery<List<DepartmentTreeNodeResponse>>;
