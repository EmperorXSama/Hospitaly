using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Common.Domain;
using Hospitaly.Modules.Clinic.Application.Abstractions.Data;
using Hospitaly.Modules.Clinic.Domain.Clinic;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.TransferClinicOwnershipPercentage;

internal sealed class TransferClinicOwnershipPercentageCommandHandler(
    IClinicRepository clinicRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<TransferClinicOwnershipPercentageCommand, Success>
{
    public async Task<ErrorOr<Success>> Handle(
        TransferClinicOwnershipPercentageCommand request,
        CancellationToken cancellationToken)
    {
        var clinic = await clinicRepository.GetByIdAsync(request.ClinicId, cancellationToken);
        if (clinic is null)
            return Error.NotFound(
                code: "TransferClinicOwnershipPercentage.NotFound",
                description: $"Clinic with id {request.ClinicId} was not found.");

        var transfers = request.Transfers
            .Select(t => (t.OwnershipId, t.SharePercentage))
            .ToList();

        var result = clinic.TransferPercentage(
            request.FromOwnershipId,
            transfers,
            request.RetainedPercentage,
            request.UserId,
            DateTimeOffset.UtcNow);
        if (result.IsError)
            return result.Errors;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
