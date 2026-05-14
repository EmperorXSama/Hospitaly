using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Common.Domain;
using Hospitaly.Modules.Clinic.Application.Abstractions.Data;
using Hospitaly.Modules.Clinic.Domain.Clinic;
using Hospitaly.Modules.Clinic.Domain.Clinic.Entities;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.AddClinicSpecialty;

internal sealed class AddClinicSpecialtyCommandHandler(
    IClinicRepository clinicRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<AddClinicSpecialtyCommand, Success>
{
    public async Task<ErrorOr<Success>> Handle(
        AddClinicSpecialtyCommand request,
        CancellationToken cancellationToken)
    {
        var clinic = await clinicRepository.GetByIdAsync(request.ClinicId, cancellationToken);
        if (clinic is null)
            return Error.NotFound(
                code: "AddClinicSpecialty.NotFound",
                description: $"Clinic with id {request.ClinicId} was not found.");

        var specialtyResult = ClinicSpecialty.Create(
            request.ClinicId, request.SpecialtyId, request.IsActive, request.ConsultationFee);
        if (specialtyResult.IsError)
            return specialtyResult.Errors;

        var addResult = clinic.AddClinicSpecialty(specialtyResult.Value);
        if (addResult.IsError)
            return addResult.Errors;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
