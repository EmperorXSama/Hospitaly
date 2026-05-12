using Hospitaly.Bff.Models;
using Hospitaly.Bff.Models.DTO;
using Hospitaly.Bff.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospitaly.Bff.Controllers;
[ApiController]
[Route("user")]
[Authorize]
public sealed class UserController(
    UserDataService userDataService,
    SessionService sessionService,
    UserRegistrationService userRegistrationService) : ControllerBase
{
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(
        [FromBody] UserRegistrationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await userRegistrationService.RegisterAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("onboarding/complete")]
    public async Task<IActionResult> CompleteOnboarding(
        [FromServices] IHttpClientFactory httpClientFactory,
        CancellationToken cancellationToken)
    {
        var sessionId = User.FindFirst("session_id")?.Value;
        if (string.IsNullOrWhiteSpace(sessionId))
            return Unauthorized();

        var session = await sessionService.GetSessionAsync(sessionId);
        if (session is null)
            return Unauthorized();

        if (session.TokenExpiresAt <= DateTime.UtcNow.AddSeconds(30))
        {
            session = await sessionService.RefreshSessionTokenAsync(session);
            if (session is null)
                return Unauthorized();
        }

        var client = httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(HttpContext.RequestServices
            .GetRequiredService<IConfiguration>()
            .GetSection("ApiUrls:Main").Value!);

        using var request = new HttpRequestMessage(HttpMethod.Post, "api/users/onboarding/complete");
        request.Headers.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", session.AccessToken);
        var response = await client.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        await userDataService.InvalidateUserDataAsync(session.UserId);

        return Ok();
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMe(CancellationToken cancellationToken)
    {
        var sessionId = User.FindFirst("session_id")?.Value;

        if (string.IsNullOrWhiteSpace(sessionId))
            return Unauthorized();

        var session = await sessionService.GetSessionAsync(sessionId);

        if (session is null)
            return Unauthorized();

        if (session.TokenExpiresAt <= DateTime.UtcNow.AddSeconds(30))
        {
            session = await sessionService.RefreshSessionTokenAsync(session);

            if (session is null)
                return Unauthorized();
        }

        var userData = await userDataService.GetUserDataAsync(
            session.UserId,
            session.AccessToken,
            cancellationToken);

        return Ok(userData);
    }
}