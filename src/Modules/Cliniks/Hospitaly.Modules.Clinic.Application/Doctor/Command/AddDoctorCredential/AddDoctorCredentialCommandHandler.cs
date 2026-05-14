using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Modules.Clinic.Application.Abstractions.Data;
using Hospitaly.Modules.Clinic.Domain.Doctor;
using Hospitaly.Modules.Clinic.Domain.Doctor.Enums;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Command.AddDoctorCredential;

internal sealed class AddDoctorCredentialCommandHandler(
    IDoctorRepository doctorRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<AddDoctorCredentialCommand, Guid>
{
    public async Task<ErrorOr<Guid>> Handle(
        AddDoctorCredentialCommand request,
        CancellationToken cancellationToken)
    {
        var doctor = await doctorRepository.GetByIdWithIncludeAsync(request.DoctorId, cancellationToken);
        if (doctor is null)
            return DoctorErrors.DoctorNotFound(request.DoctorId);

        if (!Enum.TryParse<CredentialType>(request.CredentialType, ignoreCase: true, out var credentialType))
            return Error.Validation(
                "Doctor.Credential.InvalidType",
                $"The credential type '{request.CredentialType}' is not valid.");

        var credentialResult = DoctorCredential.Create(
            request.DoctorId,
            credentialType,
            request.IssuingAuthority,
            request.DocumentNumber,
            new DateTimeOffset(request.IssueDate, TimeSpan.Zero),
            new DateTimeOffset(request.ExpiryDate, TimeSpan.Zero),
            request.UserId,
            DateTime.UtcNow);

        if (credentialResult.IsError)
            return credentialResult.Errors;

        doctor.AddCredential(credentialResult.Value);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return credentialResult.Value.Id;
    }
}
