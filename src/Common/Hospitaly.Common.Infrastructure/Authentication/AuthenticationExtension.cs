using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace Hospitaly.Common.Infrastructure.Authentication;

internal static class AuthenticationExtension
{
    internal static IServiceCollection AddAuthenticationInternal(this IServiceCollection service)
    {
        service.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
            {
            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = context =>
                {
                    Console.WriteLine("TOKEN VALIDATED");
                    return Task.CompletedTask;
                },
                OnAuthenticationFailed = async context =>
                {
                    Console.WriteLine("TOKEN Failed");
                }
            };
        });

        service.AddHttpContextAccessor();
        service.ConfigureOptions<JwtBearerConfigureOptions>();

        return service;
    }
}