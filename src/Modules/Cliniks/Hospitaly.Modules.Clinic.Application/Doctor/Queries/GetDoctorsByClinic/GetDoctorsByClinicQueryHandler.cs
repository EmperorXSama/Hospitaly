using Dapper;
using ErrorOr;
using Hospitaly.Common.Application;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Common.Application.Data;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Queries.GetDoctorsByClinic;

internal sealed class GetDoctorsByClinicQueryHandler(IDbConnectionFactory factory)
    : IQueryHandler<GetDoctorsByClinicQuery, PaginatedResult<DoctorAffiliationSummaryResponse>>
{
    public async Task<ErrorOr<PaginatedResult<DoctorAffiliationSummaryResponse>>> Handle(
        GetDoctorsByClinicQuery request,
        CancellationToken cancellationToken)
    {
        await using var connection = await factory.OpenConnectionAsync();

        var page = Math.Max(request.Page, 1);
        var pageSize = Math.Clamp(request.PageSize, 1, 100);
        var offset = (page - 1) * pageSize;

        var whereClauses = new List<string> { @"ca.""ClinicId"" = @ClinicId" };
        var parameters = new DynamicParameters();
        parameters.Add("ClinicId", request.ClinicId);

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            whereClauses.Add(@"ca.""Status"" = @Status");
            parameters.Add("Status", request.Status);
        }

        var where = "WHERE " + string.Join(" AND ", whereClauses);

        var countSql = $"""
            SELECT COUNT(*)
            FROM clinics."ClinicAffiliations" ca
            {where}
            """;

        var totalCount = await connection.ExecuteScalarAsync<int>(countSql, parameters);

        var dataSql = $"""
            SELECT
                d."Id" AS DoctorId,
                d."Title" AS Title,
                d."Status" AS DoctorStatus,
                ca."Id" AS AffiliationId,
                ca."Status" AS AffiliationStatus,
                ca."JoinedDate" AS JoinedDate,
                ca."DepartmentId" AS DepartmentId
            FROM clinics."ClinicAffiliations" ca
            JOIN clinics."Doctors" d ON d."Id" = ca."DoctorId"
            {where}
            ORDER BY ca."JoinedDate" DESC
            LIMIT @PageSize OFFSET @Offset
            """;
        parameters.Add("PageSize", pageSize);
        parameters.Add("Offset", offset);

        var items = (await connection.QueryAsync<DoctorAffiliationSummaryResponse>(dataSql, parameters)).ToList();

        return new PaginatedResult<DoctorAffiliationSummaryResponse>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}
