using Hospitaly.Common.Infrastructure.Authentication;
using Hospitaly.Common.Presentation;
using Hospitaly.Modules.Clinic.Application.Clinic.Commands.CreateDoctor;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospitaly.Modules.Clinic.Presentation;

[ApiController]
[Route("doctors")]
public class DoctorController(ISender sender) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Create()
    {
        var command = new CreateDoctorCommand(HttpContext.User.GetUserId());
        var result = await sender.Send(command);
        return Ok(result.ToApiResponse());
    }
}
