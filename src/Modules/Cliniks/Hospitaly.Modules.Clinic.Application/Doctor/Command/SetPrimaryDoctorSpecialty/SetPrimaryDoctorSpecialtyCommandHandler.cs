using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Modules.Clinic.Application.Abstractions.Data;
using Hospitaly.Modules.Clinic.Domain.Doctor;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Command.SetPrimaryDoctorSpecialty;

internal sealed class SetPrimaryDoctorSpecialtyCommandHandler(
    IDoctorRepository doctorRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<SetPrimaryDoctorSpecialtyCommand, Success>
{
    public async Task<ErrorOr<Success>> Handle(
        SetPrimaryDoctorSpecialtyCommand request,
        CancellationToken cancellationToken)
    {
        var doctor = await doctorRepository.GetByIdWithIncludeAsync(request.DoctorId, cancellationToken);
        if (doctor is null)
            return DoctorErrors.DoctorNotFound(request.DoctorId);

        var result = doctor.SetPrimarySpecialty(request.SpecialtyId);
        if (result.IsError)
            return result.Errors;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
