using Dapper;
using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Common.Application.Data;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Queries.GetClinicDepartments;

internal sealed class GetClinicDepartmentsQueryHandler(IDbConnectionFactory factory)
    : IQueryHandler<GetClinicDepartmentsQuery, List<DepartmentTreeNodeResponse>>
{
    public async Task<ErrorOr<List<DepartmentTreeNodeResponse>>> Handle(
        GetClinicDepartmentsQuery request,
        CancellationToken cancellationToken)
    {
        await using var connection = await factory.OpenConnectionAsync();

        const string sql = """
            SELECT
                d."Id" AS Id,
                d."Name" AS Name,
                d."Code" AS Code,
                d."IsActive" AS IsActive,
                d."ParentId" AS ParentId
            FROM clinics."Departments" d
            WHERE d."ClinicId" = @ClinicId
            ORDER BY d."Name"
            """;

        var flat = (await connection.QueryAsync<DepartmentRow>(sql, new { request.ClinicId })).ToList();

        var lookup = flat.ToDictionary(x => x.Id);
        var roots = new List<DepartmentTreeNodeResponse>();

        foreach (var row in flat)
        {
            var node = new DepartmentTreeNodeResponse(row.Id, row.Name, row.Code, row.IsActive, row.ParentId, []);
            lookup[row.Id] = row; // keep row for parent lookup
            _nodeCache[row.Id] = node;

            if (row.ParentId is null || !lookup.ContainsKey(row.ParentId.Value))
            {
                roots.Add(node);
            }
        }

        // Second pass: assign children
        foreach (var row in flat.Where(r => r.ParentId is not null))
        {
            if (_nodeCache.TryGetValue(row.ParentId!.Value, out var parent))
            {
                parent.Children.Add(_nodeCache[row.Id]);
            }
        }

        return roots;
    }

    private readonly Dictionary<Guid, DepartmentTreeNodeResponse> _nodeCache = new();

    private sealed record DepartmentRow(Guid Id, string Name, string Code, bool IsActive, Guid? ParentId);
}
