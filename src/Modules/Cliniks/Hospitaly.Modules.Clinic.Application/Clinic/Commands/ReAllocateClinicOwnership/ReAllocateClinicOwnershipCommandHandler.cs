using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Common.Domain;
using Hospitaly.Common.Domain.Common.ValueObjects;
using Hospitaly.Modules.Clinic.Application.Abstractions.Data;
using Hospitaly.Modules.Clinic.Domain.Clinic;
using Hospitaly.Modules.Clinic.Domain.Clinic.Entities;
using Hospitaly.Modules.Clinic.Domain.Clinic.ValueObjects;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.ReAllocateClinicOwnership;

internal sealed class ReAllocateClinicOwnershipCommandHandler(
    IClinicRepository clinicRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<ReAllocateClinicOwnershipCommand, Success>
{
    public async Task<ErrorOr<Success>> Handle(
        ReAllocateClinicOwnershipCommand request,
        CancellationToken cancellationToken)
    {
        var clinic = await clinicRepository.GetByIdAsync(request.ClinicId, cancellationToken);
        if (clinic is null)
            return Error.NotFound(
                code: "ReAllocateClinicOwnership.NotFound",
                description: $"Clinic with id {request.ClinicId} was not found.");

        var errors = new List<Error>();
        var ownerships = new List<ClinicOwnerShip>();
        var audit = new AuditInfo(request.UserId, DateTimeOffset.UtcNow);

        foreach (var owner in request.Owners)
        {
            var rangeResult = DateTimeRange.Create(owner.EffectiveStart, owner.EffectiveEnd ?? owner.EffectiveStart.AddYears(10));
            if (rangeResult.IsError)
            {
                errors.AddRange(rangeResult.Errors);
                continue;
            }

            var ownershipResult = ClinicOwnerShip.Create(
                audit,
                owner.OwnerId,
                owner.OwnerShipType,
                owner.SharePercentage,
                new OwnershipEffectiveRange(rangeResult.Value),
                owner.Status);
            if (ownershipResult.IsError)
            {
                errors.AddRange(ownershipResult.Errors);
                continue;
            }

            ownerships.Add(ownershipResult.Value);
        }

        if (errors.Count != 0)
            return errors;

        var updateResult = clinic.ReAllocateOwnership(ownerships, request.UserId, DateTimeOffset.UtcNow);
        if (updateResult.IsError)
            return updateResult.Errors;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
