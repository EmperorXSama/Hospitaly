using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Common.Domain;
using Hospitaly.Modules.Clinic.Application.Abstractions.Data;
using Hospitaly.Modules.Clinic.Domain.Clinic;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.RemoveClinicSpecialty;

internal sealed class RemoveClinicSpecialtyCommandHandler(
    IClinicRepository clinicRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<RemoveClinicSpecialtyCommand, Success>
{
    public async Task<ErrorOr<Success>> Handle(
        RemoveClinicSpecialtyCommand request,
        CancellationToken cancellationToken)
    {
        var clinic = await clinicRepository.GetByIdAsync(request.ClinicId, cancellationToken);
        if (clinic is null)
            return Error.NotFound(
                code: "RemoveClinicSpecialty.NotFound",
                description: $"Clinic with id {request.ClinicId} was not found.");

        var result = clinic.RemoveClinicSpecialty(request.SpecialtyId);
        if (result.IsError)
            return result.Errors;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
