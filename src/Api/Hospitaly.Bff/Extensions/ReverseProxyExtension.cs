using System.Net.Http.Headers;
using Hospitaly.Bff.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using Yarp.ReverseProxy.Transforms;

namespace Hospitaly.Bff.Extensions;

internal static class ReverseProxyExtension
{
    internal static WebApplicationBuilder AddReverseProxy(this WebApplicationBuilder builder)
    {

      
        var reverseProxy = builder.Services.AddReverseProxy()
            .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
            .AddTransforms(transformBuilder =>
            {
                transformBuilder.AddRequestTransform(async context =>
                {
                    var sessionId = context.HttpContext.User.FindFirst("session_id")?.Value;

                    if (string.IsNullOrEmpty(sessionId))
                    {
                        context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return;
                    }
                    
                    var redis = context.HttpContext.RequestServices
                        .GetRequiredService<IConnectionMultiplexer>();
                    SessionService sessionService = context.HttpContext.RequestServices
                        .GetRequiredService<SessionService>();
                    
                    var session = await sessionService.GetSessionAsync(sessionId);
                    if (session is null)
                    {
                        context.HttpContext.Response.StatusCode = 401;
                        return;
                    }
                    if (session.TokenExpiresAt <= DateTime.UtcNow.AddSeconds(30))
                    {
                        session = await sessionService.RefreshSessionTokenAsync(session, context.HttpContext.RequestAborted);
                        if (session is null)
                        {
                            context.HttpContext.Response.StatusCode = 401;
                            return;
                        }
                    }
                    context.ProxyRequest.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer", session.AccessToken);
                });
            });
        if (builder.Environment.IsDevelopment())
        {
            reverseProxy.ConfigureHttpClient((_, handler) =>
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.WriteLine(">>> Configuring YARP HTTP client - skipping cert validation");
                Console.ResetColor();
                handler.SslOptions.RemoteCertificateValidationCallback = (_, _, _, _) => true;
            });
        }

        return builder;
    }
}
