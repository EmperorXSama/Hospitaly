using System.Security.Claims;
using Hospitaly.Bff.Controllers;
using Hospitaly.Bff.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Hospitaly.Bff.Extensions;

internal static class DependencyInjection
{
    internal static WebApplicationBuilder AddAuthenticationInternal(this WebApplicationBuilder builder)
    {
        
            var configuration = builder.Configuration;
            builder.Services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(configuration["Redis:Connection"]!)
            );
            builder.Services.AddScoped<SessionService>();
            builder.Services.Configure<OpenIdConnectSettings>(configuration.GetSection("OpenIdConnect"));
            builder.Services.AddHttpClient<IKeycloakTokenClient, KeycloakTokenClient>((serviceProvider, options) =>
            {
                var oidcSettings = serviceProvider.GetRequiredService<IOptionsMonitor<OpenIdConnectSettings>>().CurrentValue;
                if (string.IsNullOrWhiteSpace(oidcSettings.Authority))
                {
                    throw new InvalidOperationException("OpenIdConnect:Authority is missing in configuration.");
                }

                options.BaseAddress = new Uri($"{oidcSettings.Authority.TrimEnd('/')}/");
                options.Timeout = TimeSpan.FromSeconds(10);
            });

            builder.Services
                .AddAuthentication(options =>
                    configuration.Bind("Authentication", options)
                )
                .AddCookie(options =>
                {
                })
                .AddOpenIdConnect(options =>
                {
                    configuration.Bind("OpenIdConnect", options);

                    options.Events = new OpenIdConnectEvents
                    {
                        OnTokenValidated = async context =>
                        {
                            var sessionService = context.HttpContext.RequestServices
                                .GetRequiredService<SessionService>();

                            var sessionId = Guid.NewGuid().ToString();

                            // add session_id claim to principal
                            var identity = context.Principal?.Identity as ClaimsIdentity;
                            identity?.AddClaim(new Claim("session_id", sessionId));

                            var userId = context.Principal?.FindFirst("sub")?.Value ?? "unknown";
                            var ip = context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                            var ua = context.HttpContext.Request.Headers.UserAgent.ToString();

                            // tokens are available here
                            var accessToken = context.TokenEndpointResponse?.AccessToken ?? string.Empty;
                            var refreshToken = context.TokenEndpointResponse?.RefreshToken ?? string.Empty;
                            var expiresIn = context.TokenEndpointResponse?.ExpiresIn;
                            var tokenExpiry = int.TryParse(expiresIn, out var seconds)
                                ? DateTime.UtcNow.AddSeconds(seconds)
                                : DateTime.UtcNow.AddMinutes(5);

                            Console.WriteLine($">>> AccessToken empty: {string.IsNullOrEmpty(accessToken)}");
                            Console.WriteLine($">>> RefreshToken empty: {string.IsNullOrEmpty(refreshToken)}");

                            await sessionService.CreateSessionAsync(
                                sessionId, userId,
                                accessToken, refreshToken, tokenExpiry,
                                ip, ua
                            );
                        }
                    };
                });
            builder.Services.AddCors(options => options.AddPolicy(
                BffController.CorsPolicyName,
                policbuilder =>
                {
                    var allowedOrigins = configuration.GetSection("CorsSettings:AllowedOrigins")
                        .Get<string[]>();

                    if (allowedOrigins is { Length: > 0 })
                    {
                        policbuilder.WithOrigins(allowedOrigins);
                    }

                    policbuilder.AllowAnyHeader();  
                    policbuilder.WithMethods("GET", "POST").AllowCredentials(); 
                }
        ));
        
        return builder;
    }
}
