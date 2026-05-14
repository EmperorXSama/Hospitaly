using Dapper;
using ErrorOr;
using Hospitaly.Common.Application;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Common.Application.Data;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Queries.SearchDoctors;

internal sealed class SearchDoctorsQueryHandler(IDbConnectionFactory factory)
    : IQueryHandler<SearchDoctorsQuery, PaginatedResult<DoctorSummaryResponse>>
{
    public async Task<ErrorOr<PaginatedResult<DoctorSummaryResponse>>> Handle(
        SearchDoctorsQuery request,
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
                d.""Title"" ILIKE @SearchTerm OR
                d.""Bio"" ILIKE @SearchTerm
            )");
            parameters.Add("SearchTerm", $"%{request.SearchTerm}%");
        }

        if (request.SpecialtyId.HasValue)
        {
            whereClauses.Add(@"EXISTS (
                SELECT 1 FROM clinics.""DoctorSpecialties"" ds
                WHERE ds.""DoctorId"" = d.""Id"" AND ds.""SpecialtyId"" = @SpecialtyId
            )");
            parameters.Add("SpecialtyId", request.SpecialtyId.Value);
        }

        if (request.ClinicId.HasValue)
        {
            whereClauses.Add(@"EXISTS (
                SELECT 1 FROM clinics.""ClinicAffiliations"" ca
                WHERE ca.""DoctorId"" = d.""Id"" AND ca.""ClinicId"" = @ClinicId
            )");
            parameters.Add("ClinicId", request.ClinicId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            whereClauses.Add(@"d.""Status"" = @Status");
            parameters.Add("Status", request.Status);
        }

        var fromClause = "clinics.\"Doctors\" d";
        var where = whereClauses.Count > 0
            ? "WHERE " + string.Join(" AND ", whereClauses)
            : "";

        var countSql = $"SELECT COUNT(*) FROM {fromClause} {where}";
        var totalCount = await connection.ExecuteScalarAsync<int>(countSql, parameters);

        var dataSql = $"""
            SELECT
                d."Id" AS Id,
                d."Status" AS Status,
                d."Title" AS Title
            FROM {fromClause}
            {where}
            ORDER BY d."CreatedOnUtc" DESC
            LIMIT @PageSize OFFSET @Offset
            """;
        parameters.Add("PageSize", pageSize);
        parameters.Add("Offset", offset);

        var items = (await connection.QueryAsync<DoctorSummaryResponse>(dataSql, parameters)).ToList();

        return new PaginatedResult<DoctorSummaryResponse>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}
