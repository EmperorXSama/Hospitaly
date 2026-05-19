using Hospitaly.Common.Infrastructure.Authentication;
using Hospitaly.Common.Presentation;
using Hospitaly.Modules.Users.Application.Abstractions.Identity;
using Hospitaly.Modules.Users.Application.Users.Commands;
using Hospitaly.Modules.Users.Application.Users.Commands.CompleteOnboarding;
using Hospitaly.Modules.Users.Application.Users.Commands.RegisterUser;
using Hospitaly.Modules.Users.Application.Users.Queries.GetCurrentUserData;
using Hospitaly.Modules.Users.Application.Users.Queries.GetUserInfo;
using Hospitaly.Modules.Users.Application.Users.Queries.SearchUsersByEmail;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospitaly.Modules.Users.Presentation;

[ApiController]
[Route("users")]
public class UserController(ISender sender) : ControllerBase
{

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult> Register([FromBody] RegisterUserRequest request)
    {
        var command = new RegisterUserCommand(
            request.Email,
            request.Password,
            request.FirstName,
            request.LastName,
            request.Sex,
            request.DateOfBirth,
            request.BloodType
        );
        var result = await sender.Send(command);

        return Ok(result.ToApiResponse());
    }

    //todo : this grap a detailed information about the logged user ! should be replaced with profile later
    [HttpGet("profile")]
    [Authorize]
    public async Task<ActionResult> GetProfile()
    {
        var query = new GetUserInfoQuery(HttpContext.User.GetIdentityId());
        var result = await sender.Send(query);

        return Ok(result.ToApiResponse());
    }
    
    
    [HttpGet("user-info")]
    [Authorize(Permissions.GetUser)]
    public async Task<ActionResult> Register()
    {
        var query = new GetUserInfoQuery(HttpContext.User.GetIdentityId());
        var result = await sender.Send(query);

        return Ok(result.ToApiResponse());
    }

    [HttpPost("onboarding/complete")]
    [Authorize]
    public async Task<ActionResult> CompleteOnboarding()
    {
        var command = new CompleteOnboardingCommand(HttpContext.User.GetUserId());
        var result = await sender.Send(command);
        return Ok(result.ToApiResponse());
    }

    [HttpGet("search-by-email")]
    [Authorize]
    public async Task<ActionResult> SearchByEmail([FromQuery] string email)
    {
        var query = new SearchUsersByEmailQuery(email);
        var result = await sender.Send(query);
        return Ok(result.ToApiResponse());
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult> GetMe()
    {
        var query = new GetCurrentUserDataQuery(HttpContext.User.GetUserId());
        var result = await sender.Send(query);

        return Ok(result.ToApiResponse());
    }  
    [HttpGet("login")]
    [AllowAnonymous]
    public async Task<ActionResult> Login(string userName, string password, [FromServices] IUserKeycloakRequests userKeycloakRequests)
    {
       var result = await userKeycloakRequests.GetUserTokens(userName, password);

        return Ok(result);
    }
}
