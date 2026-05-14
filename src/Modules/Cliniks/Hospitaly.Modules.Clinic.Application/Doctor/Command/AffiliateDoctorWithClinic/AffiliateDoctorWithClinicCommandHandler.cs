using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Modules.Clinic.Application.Abstractions.Data;
using Hospitaly.Modules.Clinic.Domain.Doctor;
using Hospitaly.Modules.Clinic.Domain.Doctor.Enums;
using Hospitaly.Modules.Clinic.Domain.Doctor.ValueObjects;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Command.AffiliateDoctorWithClinic;

internal sealed class AffiliateDoctorWithClinicCommandHandler(
    IDoctorRepository doctorRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<AffiliateDoctorWithClinicCommand, Guid>
{
    public async Task<ErrorOr<Guid>> Handle(
        AffiliateDoctorWithClinicCommand request,
        CancellationToken cancellationToken)
    {
        var doctor = await doctorRepository.GetByIdWithIncludeAsync(request.DoctorId, cancellationToken);
        if (doctor is null)
            return DoctorErrors.DoctorNotFound(request.DoctorId);

        List<Privilege>? privileges = null;
        if (request.GrantedPrivileges is not null && request.GrantedPrivileges.Count != 0)
        {
            privileges = [];
            foreach (var item in request.GrantedPrivileges)
            {
                if (!Enum.TryParse<PrivilegeType>(item.PrivilegeType, ignoreCase: true, out var privilegeType))
                {
                    return Error.Validation(
                        "Doctor.Affiliation.InvalidPrivilegeType",
                        $"The privilege type '{item.PrivilegeType}' is not valid.");
                }

                var privilegeResult = Privilege.Create(privilegeType, item.GrantedAt, request.UserId);
                if (privilegeResult.IsError)
                    return privilegeResult.Errors;

                privileges.Add(privilegeResult.Value);
            }
        }

        var affiliationResult = ClinicAffiliation.Create(
            request.ClinicId,
            request.DoctorId,
            request.JoinedDate,
            request.DepartmentId,
            privileges,
            request.UserId,
            DateTime.UtcNow);

        if (affiliationResult.IsError)
            return affiliationResult.Errors;

        var addResult = doctor.AddAffiliation(affiliationResult.Value);
        if (addResult.IsError)
            return addResult.Errors;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return affiliationResult.Value.Id;
    }
}
