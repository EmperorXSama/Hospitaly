using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Common.Domain;
using Hospitaly.Common.Domain.Common.ValueObjects;
using Hospitaly.Modules.Clinic.Application.Abstractions.Data;
using Hospitaly.Modules.Clinic.Domain.Clinic;
using Hospitaly.Modules.Clinic.Domain.Clinic.Entities;
using Hospitaly.Modules.Clinic.Domain.Clinic.Enum;
using Hospitaly.Modules.Clinic.Domain.Clinic.ValueObjects;
using PublicApi;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.TransferClinicOwnershipToUser;
internal sealed class TransferClinicOwnershipToUserCommandHandler(
    IClinicRepository clinicRepository,
    IUnitOfWork unitOfWork,
    IUserApi userApi)
    : ICommandHandler<TransferClinicOwnershipToUserCommand, Success>
{
    public async Task<ErrorOr<Success>> Handle(
        TransferClinicOwnershipToUserCommand request,
        CancellationToken cancellationToken)
    {
        var clinic = await clinicRepository.GetByIdWithInclude(request.ClinicId, cancellationToken);
        if (clinic is null)
            return Error.NotFound(
                code: "TransferClinicOwnershipToUser.NotFound",
                description: $"Clinic with id {request.ClinicId} was not found.");

        var targetUser = await userApi.GetUserDataByIdentityIdAsync(request.TargetOwnerIdentityId, cancellationToken);
        if (targetUser is null)
            return Error.NotFound(
                code: "TransferClinicOwnershipToUser.TargetNotFound",
                description: $"Target user with identity {request.TargetOwnerIdentityId} was not found.");

        if (!Enum.TryParse<OwnerShipType>(request.OwnerShipType, out var ownershipType))
            return Error.Validation(
                code: "TransferClinicOwnershipToUser.InvalidOwnerShipType",
                description: $"Invalid ownership type '{request.OwnerShipType}'. Valid values: SoleOwner, CoOwner, InvestorOwner.");

        var rangeResult = DateTimeRange.Create(request.EffectiveStart, request.EffectiveStart.AddYears(10));
        if (rangeResult.IsError)
            return rangeResult.Errors;

        var transferResult = clinic.TransferOwnershipToNewOwner(
            fromOwnershipId: request.FromOwnershipId,
            targetOwnerId: Guid.Parse(targetUser.IdentityId),
            ownershipType: ownershipType,
            percentageToTransfer: request.PercentageToTransfer,
            effectiveRange: new OwnershipEffectiveRange(rangeResult.Value),
            updatedById: request.UserId,
            updatedOn: DateTimeOffset.UtcNow);

        if (transferResult.IsError)
            return transferResult.Errors;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}