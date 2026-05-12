using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Hospitaly.Common.Infrastructure.Authorization;

internal static class AuthorizationExtension
{
    internal static IServiceCollection AddAuthorizationInternal(this IServiceCollection service)
    {
        service.AddAuthorization();
        service.AddTransient<IClaimsTransformation, CustomClaimsTransformation>();
        service.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
        service.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        return service;
    }
}