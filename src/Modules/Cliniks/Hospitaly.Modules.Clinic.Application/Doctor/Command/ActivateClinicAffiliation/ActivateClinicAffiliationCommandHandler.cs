using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Modules.Clinic.Application.Abstractions.Data;
using Hospitaly.Modules.Clinic.Domain.Doctor;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Command.ActivateClinicAffiliation;

internal sealed class ActivateClinicAffiliationCommandHandler(
    IDoctorRepository doctorRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<ActivateClinicAffiliationCommand, Success>
{
    public async Task<ErrorOr<Success>> Handle(
        ActivateClinicAffiliationCommand request,
        CancellationToken cancellationToken)
    {
        var doctor = await doctorRepository.GetByIdWithIncludeAsync(request.DoctorId, cancellationToken);
        if (doctor is null)
            return DoctorErrors.DoctorNotFound(request.DoctorId);

        var result = doctor.ActivateAffiliation(request.ClinicId);
        if (result.IsError)
            return result.Errors;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
