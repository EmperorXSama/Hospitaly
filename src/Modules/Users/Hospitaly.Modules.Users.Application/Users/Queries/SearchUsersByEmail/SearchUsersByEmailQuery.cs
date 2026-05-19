using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Modules.Users.Application.Users.Queries.GetUserInfo;

namespace Hospitaly.Modules.Users.Application.Users.Queries.SearchUsersByEmail;

public sealed record SearchUsersByEmailQuery(string Email) : IQuery<List<UserResponse>>;
