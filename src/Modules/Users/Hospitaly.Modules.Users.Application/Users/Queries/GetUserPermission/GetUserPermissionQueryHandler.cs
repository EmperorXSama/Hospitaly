using Dapper;
using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Common.Application.Authorization;
using Hospitaly.Common.Application.Data;
using Hospitaly.Modules.Users.Domain.Users;

namespace Hospitaly.Modules.Users.Application.Users.Queries.GetUserPermission;

public sealed class GetUserPermissionQueryHandler(IDbConnectionFactory dbConnectionFactory) : IQueryHandler<GetUserPermissionQuery,PermissionsResponse>
{
    public async  Task<ErrorOr<PermissionsResponse>> Handle(GetUserPermissionQuery request, CancellationToken cancellationToken)
    {
        await using var connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT DISTINCT
                 u."Id" AS {nameof(UserPermission.UserId)},
                 rp."PermissionCode" AS {nameof(UserPermission.Permission)}
             FROM users."Users" u
                JOIN users."UserRoles" ur ON ur."UserId" = u."Id"
             JOIN users."RolePermissions" rp ON rp."RoleName" = ur."RolesName"
             WHERE u."IdentityId" = @IdentityId
             """;
        List<UserPermission> permissions = (await connection.QueryAsync<UserPermission>(sql, new { IdentityId = request.IdentityId })).AsList();
        if (!permissions.Any())
        {
            return UserErrors.UserNotFound(request.IdentityId);
        }

        return new PermissionsResponse(permissions[0].UserId, permissions.Select(p => p.Permission).ToHashSet());
    }

    internal sealed class UserPermission
    {
        internal Guid UserId { get; init; }

        internal string Permission { get; init; }
    }
}