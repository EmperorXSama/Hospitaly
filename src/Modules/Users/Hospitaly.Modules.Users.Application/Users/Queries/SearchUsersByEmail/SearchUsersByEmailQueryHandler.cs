using Dapper;
using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Common.Application.Data;
using Hospitaly.Modules.Users.Application.Users.Queries.GetUserInfo;

namespace Hospitaly.Modules.Users.Application.Users.Queries.SearchUsersByEmail;

internal sealed class SearchUsersByEmailQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<SearchUsersByEmailQuery, List<UserResponse>>
{
    public async Task<ErrorOr<List<UserResponse>>> Handle(SearchUsersByEmailQuery request, CancellationToken cancellationToken)
    {
        await using var connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT
                 "Id" AS "{nameof(UserResponse.UserId)}",
                 "Email" AS "{nameof(UserResponse.Email)}",
                 "FirsName" AS "{nameof(UserResponse.FirstName)}",
                 "LastName" AS "{nameof(UserResponse.LastName)}",
                 "IdentityId" AS "{nameof(UserResponse.IdentityId)}",
                 "Sex" AS "{nameof(UserResponse.Sex)}",
                 "DateOfBirth" AS "{nameof(UserResponse.DateOfBirth)}",
                 "BloodType" AS "{nameof(UserResponse.BloodType)}",
                 "CreatedOnUtc" AS "{nameof(UserResponse.CreatedOnUtc)}",
                 "RequiresOnboarding" AS "{nameof(UserResponse.RequiresOnboarding)}"
             FROM users."Users"
             WHERE "Email" ILIKE '%' || @Email || '%'
             ORDER BY "Email"
             LIMIT 20
             """;

        var users = (await connection.QueryAsync<UserResponse>(sql, new { request.Email })).ToList();
        return users;
    }
}
