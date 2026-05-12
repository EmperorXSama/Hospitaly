using Hospitaly.Bff.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospitaly.Bff.Controllers;

[ApiController]
[Route("sessions")]
[Authorize]
public class SessionsController(SessionService sessionService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetSessions()
    {
        var userId = User.FindFirst("sub")?.Value;
        var currentSessionId = User.FindFirst("session_id")?.Value;

        if (userId is null) return Unauthorized();

        var sessions = await sessionService.GetUserSessionsAsync(userId, currentSessionId ?? string.Empty);
        return Ok(sessions);
    }

    [HttpDelete("{sessionId}")]
    public async Task<IActionResult> RevokeSession(string sessionId)
    {
        var userId = User.FindFirst("sub")?.Value;
        var currentSessionId = User.FindFirst("session_id")?.Value;

        if (userId is null) return Unauthorized();

        var revoked = await sessionService.RevokeSessionAsync(sessionId, userId);
        if (!revoked) return NotFound();

        if (sessionId == currentSessionId)
            await HttpContext.SignOutAsync();

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> RevokeAllSessions()
    {
        var userId = User.FindFirst("sub")?.Value;
        if (userId is null) return Unauthorized();

        await sessionService.RevokeAllSessionsAsync(userId);
        await HttpContext.SignOutAsync();

        return NoContent();
    }



}