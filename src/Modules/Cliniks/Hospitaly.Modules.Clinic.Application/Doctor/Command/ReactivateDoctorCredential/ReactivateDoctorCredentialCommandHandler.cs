using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Modules.Clinic.Application.Abstractions.Data;
using Hospitaly.Modules.Clinic.Domain.Doctor;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Command.ReactivateDoctorCredential;

internal sealed class ReactivateDoctorCredentialCommandHandler(
    IDoctorRepository doctorRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<ReactivateDoctorCredentialCommand, Success>
{
    public async Task<ErrorOr<Success>> Handle(
        ReactivateDoctorCredentialCommand request,
        CancellationToken cancellationToken)
    {
        var doctor = await doctorRepository.GetByIdWithIncludeAsync(request.DoctorId, cancellationToken);
        if (doctor is null)
            return DoctorErrors.DoctorNotFound(request.DoctorId);

        var result = doctor.ReactivateCredential(request.CredentialId);
        if (result.IsError)
            return result.Errors;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
