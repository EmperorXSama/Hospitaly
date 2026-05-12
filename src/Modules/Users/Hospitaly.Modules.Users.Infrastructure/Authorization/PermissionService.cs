using ErrorOr;
using Hospitaly.Common.Application.Authorization;
using Hospitaly.Modules.Users.Application.Users.Queries.GetUserPermission;
using MediatR;

namespace Hospitaly.Modules.Users.Infrastructure.Authorization;
internal sealed class PermissionService(ISender sender) : IPermissionsService
{
    //note : Hello me ! this will be called on every request which is :/ not good yeah? 
    // for showcasing its fine but rememeber that hitting database on every request in this senario wont be necessary 
    // WE NEED CACHE
    public async Task<ErrorOr<PermissionsResponse>> GetUserPermissions(string identityId)
    {
        return await sender.Send(new GetUserPermissionQuery(identityId));
    }
    
    
    /*
    *note: why caching I ASK , here is the chain of the authenication request :
     * here is the chain of the request authentication flow i implemented
     HTTP Request arrives
      *  │
      *  ├── [1] AuthenticateAsync → validates JWT signature, builds ClaimsPrincipal
      *  │
      *  ├── [2] CustomClaimsTransformation.TransformAsync()
      *  │       ├── checks guard (HasClaim Sub?) → NO, first time this request
      *  │       ├── calls IPermissionsService.GetUserPermissions(identityId)
      *  │       │       └── [3] PermissionService → sends GetUserPermissionsQuery
      *  │       │               └── [4] hits DB → returns permissions list
      *  │       ├── adds Permission claims to principal
      *  │       └── adds Sub claim (sentinel for guard)
      *  │
       * ├── [5] Request hits [HasPermission("appointments:approve")]
       * │       └── PermissionAuthorizationPolicyProvider.GetPolicyAsync()
       * │               ├── checks cache → found or builds PermissionRequirement
       * │               └── [6] PermissionAuthorizationHandler.HandleRequirementAsync()
       * │                       └── reads Permission claims from principal → Succeed/Fail
      *  │
       * └── Controller executes
     *
     * SO : a user permission won't change frequently so if he made a 60 request a day then we don't want to fetch the user permission 60 times
     * db cries . so we cache for fast and easy access in a cache container
     * we can add a cache invalidator if we change permission 
     */
}
