using Hospitaly.Common.Infrastructure.Authentication;
using Hospitaly.Common.Presentation;
using Hospitaly.Modules.Clinic.Application.Clinic.Commands.AddClinicSpecialty;
using Hospitaly.Modules.Clinic.Application.Clinic.Commands.AddDepartment;
using Hospitaly.Modules.Clinic.Application.Clinic.Commands.ApplyClinicOwnershipEndDate;
using Hospitaly.Modules.Clinic.Application.Clinic.Commands.CreateClinic;
using Hospitaly.Modules.Clinic.Application.Clinic.Commands.ExpireClinicOwnership;
using Hospitaly.Modules.Clinic.Application.Clinic.Commands.ReAllocateClinicOwnership;
using Hospitaly.Modules.Clinic.Application.Clinic.Commands.RemoveClinicSpecialty;
using Hospitaly.Modules.Clinic.Application.Clinic.Commands.TerminateClinicOwnership;
using Hospitaly.Modules.Clinic.Application.Clinic.Commands.TransferClinicOwnershipPercentage;
using Hospitaly.Modules.Clinic.Application.Clinic.Commands.TransferClinicOwnershipToUser;
using Hospitaly.Modules.Clinic.Application.Clinic.Commands.UpdateClinicOwnerShare;
using Hospitaly.Modules.Clinic.Application.Clinic.Commands.UpdateClinicSpecialty;
using Hospitaly.Modules.Clinic.Application.Clinic.Commands.ReplaceOperatingLicense;
using Hospitaly.Modules.Clinic.Application.Clinic.Commands.SetDepartmentActiveState;
using Hospitaly.Modules.Clinic.Application.Clinic.Commands.UpdateDepartment;
using Hospitaly.Modules.Clinic.Application.Clinic.Commands.SetClinicOperatingHours;
using Hospitaly.Modules.Clinic.Application.Clinic.Commands.UpdateClinicAddress;
using Hospitaly.Modules.Clinic.Application.Clinic.Commands.UpdateClinicContactInfo;
using Hospitaly.Modules.Clinic.Application.Clinic.Commands.UpdateClinicInfo;
using Hospitaly.Modules.Clinic.Application.Clinic.Commands.UpdateOperatingLicenseStatus;
using Hospitaly.Modules.Clinic.Application.Clinic.Queries.GetClinicById;
using Hospitaly.Modules.Clinic.Application.Clinic.Queries.GetClinicDepartments;
using Hospitaly.Modules.Clinic.Application.Clinic.Queries.GetClinicOperatingHours;
using Hospitaly.Modules.Clinic.Application.Clinic.Queries.GetClinicOperatingLicense;
using Hospitaly.Modules.Clinic.Application.Clinic.Queries.GetClinicOwnerships;
using Hospitaly.Modules.Clinic.Application.Clinic.Queries.GetClinicSpecialties;
using Hospitaly.Modules.Clinic.Application.Clinic.Queries.GetMyClinics;
using Hospitaly.Modules.Clinic.Application.Clinic.Queries.SearchClinics;
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
        var commandWithUser = command with { UserId = Guid.Parse(HttpContext.User.GetIdentityId()) };
        var result = await sender.Send(commandWithUser);
        return Ok(result.ToApiResponse());
    }

    [HttpPatch("{clinicId:guid}/ownerships/{ownershipId:guid}/end-date")]
    [Authorize]
    public async Task<ActionResult> ApplyOwnershipEndDate(Guid clinicId, Guid ownershipId, [FromBody] ApplyClinicOwnershipEndDateCommand command)
    {
        var commandWithUser = command with
        {
            ClinicId = clinicId,
            OwnershipId = ownershipId,
            UserId = Guid.Parse(HttpContext.User.GetIdentityId())
        };
        var result = await sender.Send(commandWithUser);
        return Ok(result.ToApiResponse());
    }

    [HttpPost("setClinicOperatingHours")]
    [Authorize]
    public async Task<ActionResult> SetClinicOperatingHours([FromBody] SetClinicOperatingHoursCommand command)
    {
        var commandWithUser = command with { UserId = HttpContext.User.GetUserId() };
        var result = await sender.Send(commandWithUser);
        return Ok(result.ToApiResponse());
    }

    [HttpGet("search")]
    [Authorize]
    public async Task<ActionResult> SearchClinics(
        [FromQuery] string? searchTerm,
        [FromQuery] string? city,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = new SearchClinicsQuery(searchTerm, city, page, pageSize);
        var result = await sender.Send(query);
        return Ok(result.ToApiResponse());
    }

    [HttpGet("{clinicId:guid}")]
    [Authorize]
    public async Task<ActionResult> GetClinicById(Guid clinicId)
    {
        var query = new GetClinicByIdQuery(clinicId);
        var result = await sender.Send(query);
        return Ok(result.ToApiResponse());
    }

    [HttpGet("getClinicOperatingHours")]
    [Authorize]
    public async Task<ActionResult> GetClinicOperatingHours(Guid clinicId)
    {
        var query = new ClinicOperatingHoursQuery(clinicId);
        var result = await sender.Send(query);
        return Ok(result.ToApiResponse());
    }

    [HttpGet("{clinicId:guid}/departments")]
    [Authorize]
    public async Task<ActionResult> GetClinicDepartments(Guid clinicId)
    {
        var query = new GetClinicDepartmentsQuery(clinicId);
        var result = await sender.Send(query);
        return Ok(result.ToApiResponse());
    }

    [HttpGet("{clinicId:guid}/ownerships")]
    [Authorize]
    public async Task<ActionResult> GetClinicOwnerships(Guid clinicId)
    {
        var query = new GetClinicOwnershipsQuery(clinicId);
        var result = await sender.Send(query);
        return Ok(result.ToApiResponse());
    }

    [HttpPost("{clinicId:guid}/ownerships/transfer-to-user")]
    [Authorize("clinics:ownership:transfer")]
    public async Task<ActionResult> TransferOwnershipToUser(Guid clinicId, [FromBody] TransferClinicOwnershipToUserCommand command)
    {
        var cmd = command with
        {
            ClinicId = clinicId,
            UserId = Guid.Parse(HttpContext.User.GetIdentityId())
        };
        var result = await sender.Send(cmd);
        return Ok(result.ToApiResponse());
    }

    [HttpGet("{clinicId:guid}/specialties")]
    [Authorize]
    public async Task<ActionResult> GetClinicSpecialties(Guid clinicId)
    {
        var query = new GetClinicSpecialtiesQuery(clinicId);
        var result = await sender.Send(query);
        return Ok(result.ToApiResponse());
    }

    [HttpGet("{clinicId:guid}/operating-license")]
    [Authorize]
    public async Task<ActionResult> GetClinicOperatingLicense(Guid clinicId)
    {
        var query = new GetClinicOperatingLicenseQuery(clinicId);
        var result = await sender.Send(query);
        return Ok(result.ToApiResponse());
    }  
    [HttpGet("my")]
    [Authorize]
    public async Task<ActionResult> GetMyClinicsList()
    {
        var query = new GetMyClinicsQuery(Guid.Parse(HttpContext.User.GetIdentityId()));
        var result = await sender.Send(query);
        return Ok(result.ToApiResponse());
    }
}
