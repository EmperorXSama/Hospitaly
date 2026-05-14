using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Common.Domain;
using Hospitaly.Modules.Clinic.Application.Abstractions.Data;
using Hospitaly.Modules.Clinic.Domain.Clinic;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.UpdateClinicSpecialty;

internal sealed class UpdateClinicSpecialtyCommandHandler(
    IClinicRepository clinicRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateClinicSpecialtyCommand, Success>
{
    public async Task<ErrorOr<Success>> Handle(
        UpdateClinicSpecialtyCommand request,
        CancellationToken cancellationToken)
    {
        var clinic = await clinicRepository.GetByIdAsync(request.ClinicId, cancellationToken);
        if (clinic is null)
            return Error.NotFound(
                code: "UpdateClinicSpecialty.NotFound",
                description: $"Clinic with id {request.ClinicId} was not found.");

        var result = clinic.UpdateClinicSpecialty(
            request.SpecialtyId, request.IsActive, request.ConsultationFee);
        if (result.IsError)
            return result.Errors;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
