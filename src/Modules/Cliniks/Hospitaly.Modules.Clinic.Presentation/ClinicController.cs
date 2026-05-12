using Hospitaly.Common.Infrastructure.Authentication;
using Hospitaly.Common.Presentation;
using Hospitaly.Modules.Clinic.Application.Clinic.Commands.CreateClinic;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospitaly.Modules.Clinic.Presentation;

[ApiController]
[Route("clinics")]
public class ClinicController(ISender sender) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Create([FromBody] CreateClinicCommand command)
    {
        var commandWithUser = command with { UserId = HttpContext.User.GetUserId() };
        var result = await sender.Send(commandWithUser);
        return Ok(result.ToApiResponse());
    }
}
