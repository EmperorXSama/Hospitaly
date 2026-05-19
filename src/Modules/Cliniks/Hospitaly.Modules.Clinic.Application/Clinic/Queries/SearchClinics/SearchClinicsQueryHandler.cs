using Dapper;
using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Common.Application.Data;
using Hospitaly.Common.Application;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Queries.SearchClinics;

internal sealed class SearchClinicsQueryHandler(IDbConnectionFactory factory)
    : IQueryHandler<SearchClinicsQuery, PaginatedResult<ClinicSummaryResponse>>
{
    public async Task<ErrorOr<PaginatedResult<ClinicSummaryResponse>>> Handle(
        SearchClinicsQuery request,
        CancellationToken cancellationToken)
    {
        await using var connection = await factory.OpenConnectionAsync();

        var page = Math.Max(request.Page, 1);
        var pageSize = Math.Clamp(request.PageSize, 1, 100);
        var offset = (page - 1) * pageSize;

        var whereClauses = new List<string>();
        var parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            whereClauses.Add(@"(
                c.""Info_Name"" ILIKE @SearchTerm OR
                c.""Info_TradingName"" ILIKE @SearchTerm
            )");
            parameters.Add("SearchTerm", $"%{request.SearchTerm}%");
        }

        if (!string.IsNullOrWhiteSpace(request.City))
        {
            whereClauses.Add(@"c.""Address_Value_City"" ILIKE @City");
            parameters.Add("City", $"%{request.City}%");
        }

        var where = whereClauses.Count > 0
            ? "WHERE " + string.Join(" AND ", whereClauses)
            : "";

        var countSql = $"""SELECT COUNT(*) FROM clinics."Clinics" c {where}""";
        var totalCount = await connection.ExecuteScalarAsync<int>(countSql, parameters);

        var dataSql = $"""
            SELECT
                c."Id" AS Id,
                c."Info_Name" AS Name,
                c."Info_TradingName" AS TradingName,
                c."Address_Value_City" AS City,
                c."Address_Value_Country" AS Country
            FROM clinics."Clinics" c
            {where}
            ORDER BY c."Info_Name"
            LIMIT @PageSize OFFSET @Offset
            """;
        parameters.Add("PageSize", pageSize);
        parameters.Add("Offset", offset);

        var items = (await connection.QueryAsync<ClinicSummaryResponse>(dataSql, parameters)).ToList();

        return new PaginatedResult<ClinicSummaryResponse>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}
