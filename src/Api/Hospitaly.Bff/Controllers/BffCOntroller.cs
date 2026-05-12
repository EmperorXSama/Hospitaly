using Hospitaly.Bff.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Hospitaly.Bff.Controllers;

[ApiController]
[Route("auth")]
public class BffController : ControllerBase
{
    public const string CorsPolicyName = "Bff";

    [HttpGet("check_session")]
    [EnableCors(CorsPolicyName)]
    public ActionResult<IDictionary<string, string>> CheckSession()
    {
        // return 401 Unauthorized to force SPA redirection to Login endpoint
        if (User.Identity?.IsAuthenticated != true)
            return Unauthorized();

        return User.Claims.ToDictionary(claim => claim.Type, claim => claim.Value);
    }

    [HttpGet("login")]
    public IActionResult Login(string? returnUrl = null)
    {
        var redirectUri = returnUrl ?? "https://localhost:4200";
        return Challenge(new AuthenticationProperties { RedirectUri = redirectUri });
    }

    [Authorize]
    [HttpGet("logout")]
    public async Task<IActionResult> Logout([FromServices] SessionService sessionService)
    {
        
        var currentSessionId = User.FindFirst("session_id")?.Value;
        if (currentSessionId is null)
            return Unauthorized();
        var userId = User.FindFirst("sub")?.Value;

        if (userId is not null)
            await sessionService.RevokeSessionAsync(currentSessionId, userId);

        var properties = new AuthenticationProperties
        {
            RedirectUri = "https://localhost:4200/"
        };

        return SignOut(
            properties,
            CookieAuthenticationDefaults.AuthenticationScheme,
            OpenIdConnectDefaults.AuthenticationScheme);
    }
    
    [HttpGet("protected-data")]
    [EnableCors(BffController.CorsPolicyName)]
    [Authorize]
    public ActionResult<object> GetProtectedData()
    {
        return Ok(new {
            message = "Hello from the protected BFF endpoint!",
            user = User.Identity?.Name,
            timestamp = DateTime.UtcNow
        });
    }
}
