using ErrorOr;

namespace Hospitaly.Common.Application.Authorization;

public interface IPermissionsService
{
    Task<ErrorOr<PermissionsResponse>> GetUserPermissions(string identityId);
}