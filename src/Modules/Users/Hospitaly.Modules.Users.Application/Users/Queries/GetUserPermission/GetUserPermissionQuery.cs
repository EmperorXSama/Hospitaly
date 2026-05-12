using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Common.Application.Authorization;

namespace Hospitaly.Modules.Users.Application.Users.Queries.GetUserPermission;

public sealed record GetUserPermissionQuery(string IdentityId):IQuery<PermissionsResponse>;