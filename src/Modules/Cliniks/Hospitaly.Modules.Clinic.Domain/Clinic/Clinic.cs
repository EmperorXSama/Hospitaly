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
    private readonly List<OperatingHours> _operatingHours = [];

    public ClinicInfo Info { get; private set; }
    public ClinicAddress Address { get; private set; }
    public OperatingLicense OperatingLicense { get; private set; }
    public ClinicContactInfo ContactInfo { get; private set; }

    public IReadOnlyCollection<ClinicSpecialty> Specialties => _specialties.AsReadOnly();
    public IReadOnlyCollection<Department> Departments => _departments.AsReadOnly();
    public IReadOnlyCollection<ClinicOwnerShip> Ownerships => _ownerships.AsReadOnly();
    public IReadOnlyCollection<OperatingHours> OperatingHours => _operatingHours.AsReadOnly();

    private Clinic()
    {
    }

    protected Clinic(AuditInfo audit) : base(audit,Guid.NewGuid())
    {
    }

    public static ErrorOr<Clinic> Create(
        ClinicInfo info,
        ClinicAddress address,
        OperatingLicense operatingLicense,
        ClinicContactInfo contactInfo,
        AuditInfo audit)
    {
        var clinic = new Clinic(audit)
        {
            Info = info,
            Address = address,
            OperatingLicense = operatingLicense,
            ContactInfo = contactInfo,
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
    // In Clinic.cs
    public ErrorOr<Success> TransferOwnershipToNewOwner(
        Guid fromOwnershipId,
        Guid targetOwnerId,
        OwnerShipType ownershipType,
        decimal percentageToTransfer,
        OwnershipEffectiveRange effectiveRange,
        Guid updatedById,
        DateTimeOffset updatedOn)
    {
        var source = _ownerships.FirstOrDefault(o => o.Id == fromOwnershipId);
        if (source is null)
            return Error.NotFound("Ownership.NotFound", $"Ownership {fromOwnershipId} not found.");

        if (source.Status != OwnerShipStatus.Active)
            return Error.Failure("Ownership.NotActive", "Only active ownerships can transfer share.");

        if (source.SharePercentage <= percentageToTransfer)
            return Error.Validation("Ownership.InvalidPercentage",
                $"Source has {source.SharePercentage}% but tried to transfer {percentageToTransfer}%.");

        var reduceResult = source.UpdateSharePercentage(
            source.SharePercentage - percentageToTransfer, updatedById, updatedOn);
        if (reduceResult.IsError)
            return reduceResult.Errors;

        // Aggregate creates the new ownership internally
        var audit = new AuditInfo(updatedById, updatedOn);
        var newOwnershipResult = ClinicOwnerShip.Create(
            audit,
            targetOwnerId,
            ownershipType,
            percentageToTransfer,
            effectiveRange,
            OwnerShipStatus.Active);

        if (newOwnershipResult.IsError)
            return newOwnershipResult.Errors;

        _ownerships.Add(newOwnershipResult.Value);

        var invariantCheck = EnforceOwnershipInvariant(_ownerships);
        if (invariantCheck.IsError)
            return invariantCheck.Errors;

        SetUpdated(updatedById, updatedOn);
        return Result.Success;
    }

    public ErrorOr<Success> UpdateOwnerShare(Guid ownershipId, decimal newSharePercentage, Guid updatedById, DateTimeOffset updatedOn)
    {
        var ownership = _ownerships.FirstOrDefault(o => o.Id == ownershipId);
        if (ownership is null)
            return Error.NotFound("Ownership.NotFound", $"Ownership with id {ownershipId} was not found.");

        var result = ownership.UpdateSharePercentage(newSharePercentage, updatedById, updatedOn);
        if (result.IsError)
            return result.Errors;

        var invariantCheck = EnforceOwnershipInvariant(_ownerships);
        if (invariantCheck.IsError)
            return invariantCheck.Errors;

        SetUpdated(updatedById, updatedOn);
        return Result.Success;
    }

    public ErrorOr<Success> ExpireOwnership(Guid ownershipId, Guid updatedById, DateTimeOffset updatedOn)
    {
        var ownership = _ownerships.FirstOrDefault(o => o.Id == ownershipId);
        if (ownership is null)
            return Error.NotFound("Ownership.NotFound", $"Ownership with id {ownershipId} was not found.");

        var result = ownership.Expire(updatedById, updatedOn);
        if (result.IsError)
            return result.Errors;

        SetUpdated(updatedById, updatedOn);
        return Result.Success;
    }

    public ErrorOr<Success> TerminateOwnership(Guid ownershipId, Guid updatedById, DateTimeOffset updatedOn)
    {
        var ownership = _ownerships.FirstOrDefault(o => o.Id == ownershipId);
        if (ownership is null)
            return Error.NotFound("Ownership.NotFound", $"Ownership with id {ownershipId} was not found.");

        var result = ownership.Terminate(updatedById, updatedOn);
        if (result.IsError)
            return result.Errors;

        SetUpdated(updatedById, updatedOn);
        return Result.Success;
    }

    public ErrorOr<Success> ApplyOwnershipEndDate(Guid ownershipId, DateTimeOffset effectiveUntil, Guid updatedById, DateTimeOffset updatedOn)
    {
        var ownership = _ownerships.FirstOrDefault(o => o.Id == ownershipId);
        if (ownership is null)
            return Error.NotFound("Ownership.NotFound", $"Ownership with id {ownershipId} was not found.");

        var result = ownership.ApplyEndDate(updatedById, effectiveUntil, updatedOn);
        if (result.IsError)
            return result.Errors;

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

    public ErrorOr<Success> UpdateInfo(ClinicInfo info, Guid updatedById, DateTimeOffset updatedOn)
    {
        Info = info;
        SetUpdated(updatedById, updatedOn);
        return Result.Success;
    }

    public ErrorOr<Success> UpdateAddress(ClinicAddress address, Guid updatedById, DateTimeOffset updatedOn)
    {
        Address = address;
        SetUpdated(updatedById, updatedOn);
        return Result.Success;
    }

    public ErrorOr<Success> UpdateContactInfo(ClinicContactInfo contactInfo, Guid updatedById, DateTimeOffset updatedOn)
    {
        ContactInfo = contactInfo;
        SetUpdated(updatedById, updatedOn);
        return Result.Success;
    }

    public ErrorOr<Success> ReplaceOperatingLicense(OperatingLicense license, Guid updatedById, DateTimeOffset updatedOn)
    {
        OperatingLicense = license;
        SetUpdated(updatedById, updatedOn);
        return Result.Success;
    }

    public ErrorOr<Success> UpdateOperatingLicenseStatus(LicenceAdministrativeStatus status, Guid updatedById, DateTimeOffset updatedOn)
    {
        var result = OperatingLicense.UpdateStatus(status, updatedById, updatedOn);
        if (result.IsError)
            return result.Errors;
        SetUpdated(updatedById, updatedOn);
        return Result.Success;
    }

    public void AddDepartment(Department department)
    {
        _departments.Add(department);
    }

    public ErrorOr<Success> UpdateDepartment(Guid departmentId, string name, string code, Guid? parentId, Guid updatedById, DateTimeOffset updatedOn)
    {
        var department = _departments.FirstOrDefault(d => d.Id == departmentId);
        if (department is null)
            return Error.NotFound("Department.NotFound", $"Department with id {departmentId} was not found.");

        var result = department.Update(name, code, parentId, updatedById, updatedOn);
        if (result.IsError)
            return result.Errors;

        SetUpdated(updatedById, updatedOn);
        return Result.Success;
    }

    public ErrorOr<Success> SetDepartmentActiveState(Guid departmentId, bool isActive, Guid updatedById, DateTimeOffset updatedOn)
    {
        var department = _departments.FirstOrDefault(d => d.Id == departmentId);
        if (department is null)
            return Error.NotFound("Department.NotFound", $"Department with id {departmentId} was not found.");

        var result = department.SetActiveState(isActive, updatedById, updatedOn);
        if (result.IsError)
            return result.Errors;

        SetUpdated(updatedById, updatedOn);
        return Result.Success;
    }

    public ErrorOr<Success> AddClinicSpecialty(ClinicSpecialty specialty)
    {
        if (_specialties.Any(s => s.SpecialtyId == specialty.SpecialtyId))
            return Error.Conflict(
                code: "ClinicSpecialty.Duplicate",
                description: $"Specialty {specialty.SpecialtyId} is already linked to this clinic.");

        _specialties.Add(specialty);
        return Result.Success;
    }

    public ErrorOr<Success> UpdateClinicSpecialty(Guid specialtyId, bool isActive, decimal? consultationFee)
    {
        var specialty = _specialties.FirstOrDefault(s => s.SpecialtyId == specialtyId);
        if (specialty is null)
            return Error.NotFound(
                code: "ClinicSpecialty.NotFound",
                description: $"Specialty {specialtyId} is not linked to this clinic.");

        specialty.Update(isActive, consultationFee);
        return Result.Success;
    }

    public ErrorOr<Success> RemoveClinicSpecialty(Guid specialtyId)
    {
        var specialty = _specialties.FirstOrDefault(s => s.SpecialtyId == specialtyId);
        if (specialty is null)
            return Error.NotFound(
                code: "ClinicSpecialty.NotFound",
                description: $"Specialty {specialtyId} is not linked to this clinic.");

        _specialties.Remove(specialty);
        return Result.Success;
    }

    public ErrorOr<Success> UpdateOperatingHours(List<OperatingHours> operatingHoursToModify, Guid requestUserId, DateTime utcNow)
    {

        foreach (var operationHour in operatingHoursToModify)
        {
            var existing = _operatingHours.FindIndex(o => o.Day == operationHour.Day);
            if (existing == -1)
            {
                _operatingHours.Add(operationHour);
            }
            else
            {
                _operatingHours[existing] = operationHour;
            }
        }
      
        SetUpdated(requestUserId, utcNow);
        return Result.Success;
    }
}