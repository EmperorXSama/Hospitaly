using ErrorOr;
using Hospitaly.Common.Domain;
using Hospitaly.Common.Domain.Common.ValueObjects;
using Hospitaly.Modules.Clinic.Domain.Clinic.Enum;
using Hospitaly.Modules.Clinic.Domain.Clinic.ValueObjects;

namespace Hospitaly.Modules.Clinic.Domain.Clinic.Entities;

public class ClinicOwnerShip : Entity
{
    public Guid OwnerId { get; private set; }
    public OwnerShipType OwnerShipType { get; set; }
    public decimal SharePercentage { get; private set; }
    public OwnershipEffectiveRange OwnershipEffectivePeriod { get; private set; }
    public OwnerShipStatus Status { get; private set; }
    
    private ClinicOwnerShip(){}
    private  ClinicOwnerShip(
        AuditInfo audit,
        Guid ownerId,
        OwnerShipType ownerShipType,
        decimal sharedPercentage,
        OwnershipEffectiveRange ownershipEffectivePeriod,
        OwnerShipStatus status) : base(audit)
    {
        OwnerId = ownerId;
        OwnerShipType = ownerShipType;
        SharePercentage = sharedPercentage;
        OwnershipEffectivePeriod = ownershipEffectivePeriod;
        Status = status;
    }

    public static ErrorOr<ClinicOwnerShip> Create(
        AuditInfo audit,
        Guid ownerId,
        OwnerShipType ownerShipType,
        decimal sharedPercentage,
        OwnershipEffectiveRange ownershipEffectivePeriod,
        OwnerShipStatus status
    )
    {
        if (sharedPercentage <= 0 || sharedPercentage > 100)
        {
            return Error.Validation(code: "InvalidSharePercentage", description: "Share percentage must be between 0 and 100.",
                metadata: new Dictionary<string, object>
                {
                    ["sharedPercentage"] = sharedPercentage
                });
        }

        if (ownerId == Guid.Empty)
        {
            return Error.Validation(code: "InvalidOwnerId", description: "Owner ID cannot be empty.",
                metadata: new Dictionary<string, object>
                {
                    ["ownerId"] = ownerId
                });
        }
        
        var clinicOwnerShip = new ClinicOwnerShip(audit, ownerId, ownerShipType, sharedPercentage, ownershipEffectivePeriod, status);
        return clinicOwnerShip;
    }
    
    
    
    public ErrorOr<Success> UpdateSharePercentage(decimal newPercentage ,Guid updatedById, DateTimeOffset updatedOn )
    {
        if (newPercentage <= 0 || newPercentage > 100)
        {
            return Error.Validation(code: "InvalidSharePercentage", description: "Share percentage must be between 0 and 100.",
                metadata: new Dictionary<string, object>
                {
                    ["sharedPercentage"] = newPercentage
                });
        }
        
        SharePercentage = newPercentage;
        SetUpdated(updatedById, updatedOn);

        return Result.Success;
    }
    public ErrorOr<Success> Expire(Guid updatedById, DateTimeOffset updatedOn)
    {
        if (!OwnershipEffectivePeriod.Range.IsEnded(updatedOn))
            return Error.Failure(
                code: "OwnershipNotExpired",
                description: "Cannot expire an ownership that hasn't reached its end date yet.");

        Status = OwnerShipStatus.Expired; 
        SetUpdated(updatedById, updatedOn);
        return Result.Success;
    }

    public ErrorOr<Success> Terminate(Guid updatedById, DateTimeOffset updatedOn)
    {
        if (Status == OwnerShipStatus.Terminated || Status == OwnerShipStatus.Expired)
            return Error.Failure(
                code: "OwnershipAlreadyEnded",
                description: "Cannot terminate an ownership that has already ended.");

        Status = OwnerShipStatus.Terminated;
        SetUpdated(updatedById, updatedOn);
        return Result.Success;
    }

    public ErrorOr<Success> ApplyEndDate(Guid updatedById, DateTimeOffset effectiveUntil, DateTimeOffset updatedOn)
    {
        if (effectiveUntil.Date == OwnershipEffectivePeriod.Range.End?.Date) 
         {
             return Error.Validation(
                 code: "EffectiveUntilUnchanged",
                 description: "The provided effective until date is the same as the current end date.",
                 metadata: new Dictionary<string, object>
                 {
                     ["effectiveUntil"] = effectiveUntil,
                     ["currentEndDate"] = OwnershipEffectivePeriod.Range.End
                 });
         }
         var rangeValue  = DateTimeRange.Create(OwnershipEffectivePeriod.Range.Start, effectiveUntil);
         if (rangeValue.IsError)
         {
             return rangeValue.Errors;
         }

         OwnershipEffectivePeriod = new OwnershipEffectiveRange(rangeValue.Value);
         SetUpdated(updatedById, updatedOn);
         return Result.Success;
     }
    
}