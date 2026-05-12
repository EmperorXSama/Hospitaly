using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Users.Application.Users.Queries.GetCurrentUserData;

public sealed record GetCurrentUserDataQuery(Guid UserId) : IQuery<UserDataDto>;
