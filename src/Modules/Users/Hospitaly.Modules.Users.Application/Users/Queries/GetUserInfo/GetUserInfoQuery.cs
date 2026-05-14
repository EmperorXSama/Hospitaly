using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Users.Application.Users.Queries.GetUserInfo;

public sealed record GetUserInfoQuery(string UserIdentity): IQuery<UserResponse>;