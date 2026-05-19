using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Modules.Clinic.Application.Abstractions.Data;
using Hospitaly.Modules.Clinic.Domain.Doctor;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Command.VerifyDoctorCredential;

internal sealed class VerifyDoctorCredentialCommandHandler(
    IDoctorRepository doctorRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<VerifyDoctorCredentialCommand, Success>
{
    public async Task<ErrorOr<Success>> Handle(
        VerifyDoctorCredentialCommand request,
        CancellationToken cancellationToken)
    {
        var doctor = await doctorRepository.GetByIdWithIncludeAsync(request.DoctorId, cancellationToken);
        if (doctor is null)
            return DoctorErrors.DoctorNotFound(request.DoctorId);

        var result = doctor.VerifyCredential(
            request.CredentialId,
            request.UserId,
            DateTime.UtcNow);

        if (result.IsError)
            return result.Errors;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
