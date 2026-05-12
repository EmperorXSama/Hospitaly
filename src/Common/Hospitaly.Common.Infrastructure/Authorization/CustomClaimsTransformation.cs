using System.Security.Claims;
using ErrorOr;
using Hospitaly.Common.Application.Authorization;
using Hospitaly.Common.Application.Exceptions;
using Hospitaly.Common.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Hospitaly.Common.Infrastructure.Authorization;

internal sealed  class CustomClaimsTransformation(IServiceScopeFactory serviceScopeFactory) : IClaimsTransformation
{
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.HasClaim(c => c.Type == CustomClaims.Sub))
        {
            return principal;
        }

        using IServiceScope scope = serviceScopeFactory.CreateScope();
        IPermissionsService permissionsService = scope.ServiceProvider.GetRequiredService<IPermissionsService>();
        string identityId = principal.GetIdentityId();
        ErrorOr<PermissionsResponse> result = await permissionsService.GetUserPermissions(identityId);
        if (result.IsError)
        {
            throw new HospitalyException(nameof(IPermissionsService.GetUserPermissions), result.FirstError);
        }

        var claimsIdentity = new ClaimsIdentity();
        claimsIdentity.AddClaim(new Claim(CustomClaims.Sub, result.Value.UserId.ToString()));
        foreach (var permission in result.Value.Permissions)
        {
            claimsIdentity.AddClaim(new Claim(CustomClaims.Permission, permission));
        }
        principal.AddIdentity(claimsIdentity);
        return principal;
    }
}