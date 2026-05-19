using Dapper;
using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Common.Application.Data;
using PublicApi;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Queries.GetClinicOwnerships;

internal sealed class GetClinicOwnershipsQueryHandler(IDbConnectionFactory factory, IUserApi userApi)
    : IQueryHandler<GetClinicOwnershipsQuery, List<ClinicOwnershipResponse>>
{
    public async Task<ErrorOr<List<ClinicOwnershipResponse>>> Handle(
        GetClinicOwnershipsQuery request,
        CancellationToken cancellationToken)
    {
        await using var connection = await factory.OpenConnectionAsync();

        const string sql = """
            SELECT
                o."Id" AS Id,
                o."OwnerId" AS OwnerId,
                o."OwnerShipType" AS OwnerShipType,
                o."SharePercentage" AS SharePercentage,
                o."OwnershipEffectivePeriod_Range_Start" AS EffectiveStart,
                o."OwnershipEffectivePeriod_Range_End" AS EffectiveEnd,
                o."Status" AS Status
            FROM clinics."ClinicOwnerships" o
            WHERE o."ClinicId" = @ClinicId
            ORDER BY o."OwnershipEffectivePeriod_Range_Start"
            """;

        var ownerships = (await connection.QueryAsync<ClinicOwnershipResponse>(sql, new { request.ClinicId })).ToList();

        var uniqueOwnerIds = ownerships.Select(o => o.OwnerId).Distinct().ToList();
        var ownerDataTasks = uniqueOwnerIds.Select(id =>
            userApi.GetUserDataByIdentityIdAsync(id.ToString(), cancellationToken));
        var ownerDataResults = await Task.WhenAll(ownerDataTasks);
        var ownerDataMap = ownerDataResults
            .Where(dto => dto is not null)
            .ToDictionary(dto => Guid.Parse(dto!.IdentityId));

        var result = ownerships.Select(o =>
        {
            var exists = ownerDataMap.TryGetValue(o.OwnerId, out var dto);
            return o with
            {
                Owner = exists
                    ? new OwnerInfo
                    {
                        UserId = dto!.UserId,
                        FirstName = dto.FirstName,
                        LastName = dto.LastName,
                        Email = dto.Email,
                        Sex = dto.Sex,
                        DateOfBirth = dto.DateOfBirth
                    }
                    : null
            };
        }).ToList();

        return result;
    }
}
