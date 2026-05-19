using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Common.Domain;
using Hospitaly.Modules.Clinic.Application.Abstractions.Data;
using Hospitaly.Modules.Clinic.Domain.Clinic;
using Hospitaly.Modules.Clinic.Domain.Clinic.ValueObjects;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.UpdateClinicContactInfo;

internal sealed class UpdateClinicContactInfoCommandHandler(
    IClinicRepository clinicRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateClinicContactInfoCommand, Success>
{
    public async Task<ErrorOr<Success>> Handle(
        UpdateClinicContactInfoCommand request,
        CancellationToken cancellationToken)
    {
        var clinic = await clinicRepository.GetByIdAsync(request.ClinicId, cancellationToken);
        if (clinic is null)
            return Error.NotFound(
                code: "UpdateClinicContactInfo.NotFound",
                description: $"Clinic with id {request.ClinicId} was not found.");

        var contactResult = ClinicContactInfo.Create(request.Phone, request.Email, request.Website);
        if (contactResult.IsError)
            return contactResult.Errors;

        var updateResult = clinic.UpdateContactInfo(contactResult.Value, request.UserId, DateTimeOffset.UtcNow);
        if (updateResult.IsError)
            return updateResult.Errors;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
