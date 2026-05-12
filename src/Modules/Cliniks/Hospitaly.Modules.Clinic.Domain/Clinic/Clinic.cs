using ErrorOr;
using Hospitaly.Common.Domain;
using Hospitaly.Modules.Clinic.Domain.Clinic.Entities;
using Hospitaly.Modules.Clinic.Domain.Clinic.Enum;
using Hospitaly.Modules.Clinic.Domain.Clinic.ValueObjects;

namespace Hospitaly.Modules.Clinic.Domain.Clinic;

public class Clinic : AggregateRoot
{
    private readonly List<ClinicSpecialty> _specialties = [];
    private readonly List<Department> _departments = [];
    private readonly List<ClinicOwnerShip> _ownerships = [];

    public ClinicInfo Info { get; private set; }
    public ClinicAddress Address { get; private set; }
    public OperatingLicense OperatingLicense { get; private set; }
    public ClinicContactInfo ContactInfo { get; private set; }
    public OperatingHours OperatingHours { get; private set; }

    public IReadOnlyCollection<ClinicSpecialty> Specialties => _specialties.AsReadOnly();
    public IReadOnlyCollection<Department> Departments => _departments.AsReadOnly();
    public IReadOnlyCollection<ClinicOwnerShip> Ownerships => _ownerships.AsReadOnly();

    private Clinic()
    {
    }

    protected Clinic(AuditInfo audit) : base(audit)
    {
    }

    public static ErrorOr<Clinic> Create(
        ClinicInfo info,
        ClinicAddress address,
        OperatingLicense operatingLicense,
        ClinicContactInfo contactInfo,
        OperatingHours operatingHours,
        AuditInfo audit)
    {
        var clinic = new Clinic(audit)
        {
            Info = info,
            Address = address,
            OperatingLicense = operatingLicense,
            ContactInfo = contactInfo,
            OperatingHours = operatingHours
        };
        return clinic;
    }

    public ErrorOr<Success> ReAllocateOwnership(
        IEnumerable<ClinicOwnerShip> ownerships,
        Guid updatedById,
        DateTimeOffset updatedOn)
    {
        var incoming = ownerships as ClinicOwnerShip[] ?? ownerships.ToArray();
    
        var enforcementResult = EnforceOwnershipInvariant(incoming);
        if (enforcementResult.IsError)
            return enforcementResult.Errors;

        var incomingIds = incoming.Select(o => o.Id).ToHashSet();

        // 1. Terminate ownerships that are no longer in the new allocation
        foreach (var existing in _ownerships.Where(o => !incomingIds.Contains(o.Id)))
        {
            var terminateResult = existing.Terminate(updatedById, updatedOn);
            if (terminateResult.IsError)
                return terminateResult.Errors;
        }

        // 2. Update existing or add new
        foreach (var incomingOwnership in incoming)
        {
            var existing = _ownerships.FirstOrDefault(o => o.Id == incomingOwnership.Id);
            if (existing is not null)
            {
                // Only update if share actually changed
                if (existing.SharePercentage != incomingOwnership.SharePercentage)
                {
                    var updateResult = existing.UpdateSharePercentage(
                        incomingOwnership.SharePercentage, updatedById, updatedOn);
                    if (updateResult.IsError)
                        return updateResult.Errors;
                }
            }
            else
            {
                _ownerships.Add(incomingOwnership);
            }
        }

        SetUpdated(updatedById, updatedOn);
        return Result.Success;
    }


    public ErrorOr<Success> TransferPercentage(
        Guid fromOwnershipId ,
        List<(Guid OwnershipId, decimal SharePercentage)> transfers,
        decimal retainedPercentage,
        Guid updatedById,
        DateTimeOffset updatedOn)
    {
        var source = _ownerships.FirstOrDefault(o => o.Id == fromOwnershipId);
        if (source is null)
            return Error.Failure("OwnershipNotFound", $"No ownership found with ID {fromOwnershipId}");
        
        var totalAllocated = retainedPercentage + transfers.Sum(t => t.SharePercentage);
        if (Math.Abs(totalAllocated - source.SharePercentage) > 0.0001m)
            return Error.Failure(
                code: "OwnershipTransferInvariantViolation",
                description: $"Allocated {totalAllocated}% must equal original {source.SharePercentage}%",
                metadata: new Dictionary<string, object>
                {
                    ["originalShare"] = source.SharePercentage,
                    ["allocatedShare"] = totalAllocated
                });
        var resolvedTargets = new List<(ClinicOwnerShip Ownership, decimal NewShare)>();
        foreach (var (ownershipId, share) in transfers)
        {
            var target = _ownerships.FirstOrDefault(o => o.Id == ownershipId);
            if (target is null)
                return Error.Failure(
                    code: "OwnershipNotFound",
                    description: $"No ownership found with ID {ownershipId}",
                    metadata: new Dictionary<string, object> { ["ownershipId"] = ownershipId });

            resolvedTargets.Add((target, target.SharePercentage + share));
        }
        source.UpdateSharePercentage(retainedPercentage,updatedById, updatedOn);
        foreach (var (ownership, newShare) in resolvedTargets)
            ownership.UpdateSharePercentage(newShare, updatedById, updatedOn);
        
        var invariantCheck = EnforceOwnershipInvariant(_ownerships);
        if (invariantCheck.IsError)
            return invariantCheck.Errors;
        SetUpdated(updatedById, updatedOn);
        return Result.Success;
    }
    

    private ErrorOr<Success> EnforceOwnershipInvariant( IEnumerable<ClinicOwnerShip> ownerships)
    {
        var total = ownerships.Where(o => o.Status == OwnerShipStatus.Active)
            .Sum(o => o.SharePercentage);

        if (total != 100)
        {
            return Error.Failure(
                    code: "OwnershipInvariantViolation",
                    description: $"Total ownership share percentage must equal 100%. Current total: {total}%",
                    metadata: new Dictionary<string, object>
                    {
                        ["total"] = total,
                    }
                );
        }

        return Result.Success;
    }
}