using Dapper;
using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Common.Application.Data;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Queries.GetMyClinics;

internal sealed class GetMyClinicsQueryHandler(IDbConnectionFactory factory)
    : IQueryHandler<GetMyClinicsQuery, List<ClinicListItemResponse>>
{
    public async Task<ErrorOr<List<ClinicListItemResponse>>> Handle(GetMyClinicsQuery request, CancellationToken cancellationToken)
    {
        await using var connection = await factory.OpenConnectionAsync();

        const string sql = """
                           SELECT DISTINCT
                               c."Id" AS ClinicId,
                               c."Info_Name" as Name,
                               co."OwnerShipType",
                               co."Status"
                           FROM clinics."ClinicOwnerships" co
                           INNER JOIN clinics."Clinics" c ON c."Id" = co."ClinicId"
                           WHERE co."OwnerId" = @UserId
                           ORDER BY c."Info_Name";
                           """;

        var rows = await connection.QueryAsync<ClinicListItemResponse>(sql, new { request.UserId });
        return rows.ToList();
    }
}
