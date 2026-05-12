using Dapper;
using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Common.Application.Data;
using Hospitaly.Modules.Users.Domain.Users;

namespace Hospitaly.Modules.Users.Application.Users.Queries.GetUserInfo;

public sealed class GetUserInfoQueryHandler(IDbConnectionFactory dbConnectionFactory) : IQueryHandler<GetUserInfoQuery,UserResponse>
{
    public async  Task<ErrorOr<UserResponse>> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        await using var connection = await dbConnectionFactory.OpenConnectionAsync();
        const string sql =
            $"""
             SELECT
                 "Id" AS {nameof(UserResponse.UserId)},
                 "Email" AS {nameof(UserResponse.Email)},
                 "FirsName" AS {nameof(UserResponse.FirstName)},
                 "LastName" AS {nameof(UserResponse.LastName)},
                 "IdentityId" AS {nameof(UserResponse.IdentityId)},
                 "Sex" AS {nameof(UserResponse.Sex)},
                 "DateOfBirth" AS {nameof(UserResponse.DateOfBirth)},
                 "BloodType" AS {nameof(UserResponse.BloodType)},
                 "CreatedOnUtc" AS {nameof(UserResponse.CreatedOnUtc)}
             FROM users."Users"
             WHERE "Id" = @UserId
             """;
        UserResponse? user = await connection.QuerySingleOrDefaultAsync<UserResponse>(sql, request);

        if (user is null)
        {
            return UserErrors.UserNotFound(request.UserId);
        }

        return user;
    }
    
}