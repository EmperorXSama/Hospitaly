using Hospitaly.Common.Infrastructure.Authentication;
using Hospitaly.Common.Presentation;
using Hospitaly.Modules.Clinic.Application.Doctor.Command.ActivateClinicAffiliation;
using Hospitaly.Modules.Clinic.Application.Doctor.Command.ActivateDoctor;
using Hospitaly.Modules.Clinic.Application.Doctor.Command.AddDoctorCredential;
using Hospitaly.Modules.Clinic.Application.Doctor.Command.AddDoctorSpecialty;
using Hospitaly.Modules.Clinic.Application.Doctor.Command.AffiliateDoctorWithClinic;
using Hospitaly.Modules.Clinic.Application.Doctor.Command.CreateDoctor;
using Hospitaly.Modules.Clinic.Application.Doctor.Command.DeactivateDoctor;
using Hospitaly.Modules.Clinic.Application.Doctor.Command.ReactivateDoctorCredential;
using Hospitaly.Modules.Clinic.Application.Doctor.Command.RemoveDoctorSpecialty;
using Hospitaly.Modules.Clinic.Application.Doctor.Command.RevokeDoctorCredential;
using Hospitaly.Modules.Clinic.Application.Doctor.Command.SetPrimaryDoctorSpecialty;
using Hospitaly.Modules.Clinic.Application.Doctor.Command.SuspendClinicAffiliation;
using Hospitaly.Modules.Clinic.Application.Doctor.Command.SuspendDoctorCredential;
using Hospitaly.Modules.Clinic.Application.Doctor.Command.UpdateDoctorProfile;
using Hospitaly.Modules.Clinic.Application.Doctor.Command.UploadDoctorAvatar;
using Hospitaly.Modules.Clinic.Application.Doctor.Command.VerifyDoctorCredential;
using Hospitaly.Modules.Clinic.Application.Doctor.Queries.GetDoctorAffiliations;
using Hospitaly.Modules.Clinic.Application.Doctor.Queries.GetDoctorById;
using Hospitaly.Modules.Clinic.Application.Doctor.Queries.GetDoctorByUserId;
using Hospitaly.Modules.Clinic.Application.Doctor.Queries.GetDoctorCredentials;
using Hospitaly.Modules.Clinic.Application.Doctor.Queries.GetDoctorSpecialties;
using Hospitaly.Modules.Clinic.Application.Doctor.Queries.GetDoctorsByClinic;
using Hospitaly.Modules.Clinic.Application.Doctor.Queries.SearchDoctors;
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

    [HttpPut("{doctorId:guid}/profile")]
    [Authorize]
    public async Task<ActionResult> UpdateProfile(
        Guid doctorId,
        [FromBody] UpdateDoctorProfileCommand command)
    {
        var cmd = command with
        {
            DoctorId = doctorId,
            UserId = HttpContext.User.GetUserId()
        };
        var result = await sender.Send(cmd);
        return Ok(result.ToApiResponse());
    }

    [HttpPut("{doctorId:guid}/avatar")]
    [Authorize]
    public async Task<ActionResult> UploadAvatar(
        Guid doctorId,
        [FromBody] UploadDoctorAvatarCommand command)
    {
        var cmd = command with
        {
            DoctorId = doctorId,
            UserId = HttpContext.User.GetUserId()
        };
        var result = await sender.Send(cmd);
        return Ok(result.ToApiResponse());
    }

    [HttpPut("{doctorId:guid}/activate")]
    [Authorize]
    public async Task<ActionResult> Activate(
        Guid doctorId)
    {
        var command = new ActivateDoctorCommand(doctorId, HttpContext.User.GetUserId());
        var result = await sender.Send(command);
        return Ok(result.ToApiResponse());
    }

    [HttpPut("{doctorId:guid}/deactivate")]
    [Authorize]
    public async Task<ActionResult> Deactivate(
        Guid doctorId)
    {
        var command = new DeactivateDoctorCommand(doctorId, HttpContext.User.GetUserId());
        var result = await sender.Send(command);
        return Ok(result.ToApiResponse());
    }

    [HttpPost("{doctorId:guid}/credentials")]
    [Authorize]
    public async Task<ActionResult> AddCredential(
        Guid doctorId,
        [FromBody] AddDoctorCredentialCommand command)
    {
        var cmd = command with
        {
            DoctorId = doctorId,
            UserId = HttpContext.User.GetUserId()
        };
        var result = await sender.Send(cmd);
        return Ok(result.ToApiResponse());
    }

    [HttpPut("{doctorId:guid}/credentials/{credentialId:guid}/verify")]
    [Authorize]
    public async Task<ActionResult> VerifyCredential(
        Guid doctorId,
        Guid credentialId,
        [FromBody] VerifyDoctorCredentialCommand command)
    {
        var cmd = command with
        {
            DoctorId = doctorId,
            CredentialId = credentialId,
            UserId = HttpContext.User.GetUserId()
        };
        var result = await sender.Send(cmd);
        return Ok(result.ToApiResponse());
    }

    [HttpPut("{doctorId:guid}/credentials/{credentialId:guid}/revoke")]
    [Authorize]
    public async Task<ActionResult> RevokeCredential(
        Guid doctorId,
        Guid credentialId,
        [FromBody] RevokeDoctorCredentialCommand command)
    {
        var cmd = command with
        {
            DoctorId = doctorId,
            CredentialId = credentialId,
            UserId = HttpContext.User.GetUserId()
        };
        var result = await sender.Send(cmd);
        return Ok(result.ToApiResponse());
    }

    [HttpPut("{doctorId:guid}/credentials/{credentialId:guid}/suspend")]
    [Authorize]
    public async Task<ActionResult> SuspendCredential(
        Guid doctorId,
        Guid credentialId,
        [FromBody] SuspendDoctorCredentialCommand command)
    {
        var cmd = command with
        {
            DoctorId = doctorId,
            CredentialId = credentialId,
            UserId = HttpContext.User.GetUserId()
        };
        var result = await sender.Send(cmd);
        return Ok(result.ToApiResponse());
    }

    [HttpPut("{doctorId:guid}/credentials/{credentialId:guid}/reactivate")]
    [Authorize]
    public async Task<ActionResult> ReactivateCredential(
        Guid doctorId,
        Guid credentialId,
        [FromBody] ReactivateDoctorCredentialCommand command)
    {
        var cmd = command with
        {
            DoctorId = doctorId,
            CredentialId = credentialId,
            UserId = HttpContext.User.GetUserId()
        };
        var result = await sender.Send(cmd);
        return Ok(result.ToApiResponse());
    }

    [HttpPost("{doctorId:guid}/specialties")]
    [Authorize]
    public async Task<ActionResult> AddSpecialties(
        Guid doctorId,
        [FromBody] AddDoctorSpecialtyCommand command)
    {
        var cmd = command with
        {
            DoctorId = doctorId,
            UserId = HttpContext.User.GetUserId()
        };
        var result = await sender.Send(cmd);
        return Ok(result.ToApiResponse());
    }

    [HttpDelete("{doctorId:guid}/specialties/{specialtyId:guid}")]
    [Authorize]
    public async Task<ActionResult> RemoveSpecialty(
        Guid doctorId,
        Guid specialtyId)
    {
        var command = new RemoveDoctorSpecialtyCommand(doctorId, specialtyId, HttpContext.User.GetUserId());
        var result = await sender.Send(command);
        return Ok(result.ToApiResponse());
    }

    [HttpPut("{doctorId:guid}/specialties/{specialtyId:guid}/primary")]
    [Authorize]
    public async Task<ActionResult> SetPrimarySpecialty(
        Guid doctorId,
        Guid specialtyId)
    {
        var command = new SetPrimaryDoctorSpecialtyCommand(doctorId, specialtyId, HttpContext.User.GetUserId());
        var result = await sender.Send(command);
        return Ok(result.ToApiResponse());
    }

    [HttpPost("{doctorId:guid}/affiliations")]
    [Authorize]
    public async Task<ActionResult> AffiliateWithClinic(
        Guid doctorId,
        [FromBody] AffiliateDoctorWithClinicCommand command)
    {
        var cmd = command with
        {
            DoctorId = doctorId,
            UserId = HttpContext.User.GetUserId()
        };
        var result = await sender.Send(cmd);
        return Ok(result.ToApiResponse());
    }

    [HttpPut("{doctorId:guid}/affiliations/{clinicId:guid}/activate")]
    [Authorize]
    public async Task<ActionResult> ActivateAffiliation(
        Guid doctorId,
        Guid clinicId)
    {
        var command = new ActivateClinicAffiliationCommand(doctorId, clinicId, HttpContext.User.GetUserId());
        var result = await sender.Send(command);
        return Ok(result.ToApiResponse());
    }

    [HttpPut("{doctorId:guid}/affiliations/{clinicId:guid}/suspend")]
    [Authorize]
    public async Task<ActionResult> SuspendAffiliation(
        Guid doctorId,
        Guid clinicId)
    {
        var command = new SuspendClinicAffiliationCommand(doctorId, clinicId, HttpContext.User.GetUserId());
        var result = await sender.Send(command);
        return Ok(result.ToApiResponse());
    }

    [HttpGet("{doctorId:guid}")]
    [Authorize]
    public async Task<ActionResult> GetById(Guid doctorId)
    {
        var query = new GetDoctorByIdQuery(doctorId);
        var result = await sender.Send(query);
        return Ok(result.ToApiResponse());
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult> GetMyProfile()
    {
        var query = new GetDoctorByUserIdQuery(HttpContext.User.GetUserId());
        var result = await sender.Send(query);
        return Ok(result.ToApiResponse());
    }

    [HttpGet("search")]
    [Authorize]
    public async Task<ActionResult> Search(
        [FromQuery] string? searchTerm,
        [FromQuery] Guid? specialtyId,
        [FromQuery] Guid? clinicId,
        [FromQuery] string? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = new SearchDoctorsQuery(searchTerm, specialtyId, clinicId, status, page, pageSize);
        var result = await sender.Send(query);
        return Ok(result.ToApiResponse());
    }

    [HttpGet("{doctorId:guid}/credentials")]
    [Authorize]
    public async Task<ActionResult> GetCredentials(Guid doctorId)
    {
        var query = new GetDoctorCredentialsQuery(doctorId);
        var result = await sender.Send(query);
        return Ok(result.ToApiResponse());
    }

    [HttpGet("{doctorId:guid}/specialties")]
    [Authorize]
    public async Task<ActionResult> GetSpecialties(Guid doctorId)
    {
        var query = new GetDoctorSpecialtiesQuery(doctorId);
        var result = await sender.Send(query);
        return Ok(result.ToApiResponse());
    }

    [HttpGet("{doctorId:guid}/affiliations")]
    [Authorize]
    public async Task<ActionResult> GetAffiliations(Guid doctorId)
    {
        var query = new GetDoctorAffiliationsQuery(doctorId);
        var result = await sender.Send(query);
        return Ok(result.ToApiResponse());
    }

    [HttpGet("by-clinic/{clinicId:guid}")]
    [Authorize]
    public async Task<ActionResult> GetByClinic(
        Guid clinicId,
        [FromQuery] string? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = new GetDoctorsByClinicQuery(clinicId, status, page, pageSize);
        var result = await sender.Send(query);
        return Ok(result.ToApiResponse());
    }
}
