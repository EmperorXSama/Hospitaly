using Hospitaly.Common.Presentation;
using Hospitaly.Modules.Clinic.Application.Specielties.Queries.GetSpecialties;
using Hospitaly.Modules.Clinic.Domain.Specialty;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospitaly.Modules.Clinic.Presentation;

[ApiController]
[Route("specialties")]
public class SpecialtyController(ISender sender) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<SpecialtiesResponse>> GetAllSpecialties()
    {
        var query = new GetSpecialtiesQuery();
        var result = await sender.Send(query);
        return Ok(result.ToApiResponse());
    }
}