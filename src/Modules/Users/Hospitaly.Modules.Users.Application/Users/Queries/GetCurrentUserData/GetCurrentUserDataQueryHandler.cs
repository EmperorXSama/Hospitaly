using Dapper;
using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Common.Application.Data;
using Hospitaly.Modules.Users.Domain.Users;

namespace Hospitaly.Modules.Users.Application.Users.Queries.GetCurrentUserData;

public sealed class GetCurrentUserDataQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetCurrentUserDataQuery, UserDataDto>
{
    public async Task<ErrorOr<UserDataDto>> Handle(GetCurrentUserDataQuery request, CancellationToken cancellationToken)
    {
        await using var connection = await dbConnectionFactory.OpenConnectionAsync();

            const string sql =
                """
                SELECT
                    u."Id" AS UserId,
                    u."FirsName" AS FirstName,
                    u."LastName" AS LastName,
                    u."Email" AS Email,
                    u."RequiresOnboarding" AS RequiresOnboarding,
                    ur."RolesName" AS Role,
                    rp."PermissionCode" AS Permission
                FROM users."Users" u
                LEFT JOIN users."UserRoles" ur ON ur."UserId" = u."Id"
                LEFT JOIN users."RolePermissions" rp ON rp."RoleName" = ur."RolesName"
                WHERE u."Id" = @UserId
                """;

        List<UserDataRow> rows = (await connection.QueryAsync<UserDataRow>(sql, request)).AsList();

        if (!rows.Any())
        {
            return UserErrors.UserNotFound(request.UserId);
        }

        UserDataRow firstRow = rows[0];

        string userName = string.Join(
            " ",
            new[] { firstRow.FirstName, firstRow.LastName }
                .Where(static value => !string.IsNullOrWhiteSpace(value))
                .Select(static value => value.Trim()));

        string[] roles = rows
            .Select(static row => row.Role)
            .Where(static role => !string.IsNullOrWhiteSpace(role))
            .Select(static role => role!)
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        string[] permissions = rows
            .Select(static row => row.Permission)
            .Where(static permission => !string.IsNullOrWhiteSpace(permission))
            .Select(static permission => permission!)
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        return new UserDataDto(
            firstRow.UserId.ToString(),
            userName,
            firstRow.Email,
            roles,
            permissions,
            firstRow.RequiresOnboarding);
    }

    private sealed record UserDataRow(
        Guid UserId,
        string FirstName,
        string LastName,
        string Email,
        bool RequiresOnboarding,
        string? Role,
        string? Permission);
}
