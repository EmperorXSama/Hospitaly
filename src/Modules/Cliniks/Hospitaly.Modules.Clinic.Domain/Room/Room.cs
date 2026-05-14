using ErrorOr;
using Hospitaly.Common.Domain;
using Hospitaly.Common.Domain.Common.ValueObjects;
using Hospitaly.Modules.Clinic.Domain.Room.Enums;
using Hospitaly.Modules.Clinic.Domain.Room.ValueObjects;

namespace Hospitaly.Modules.Clinic.Domain.Room;

public class Room : AggregateRoot
{
    private readonly List<RoomCapability> _capabilities = [];
    private readonly List<MaintenanceBlock> _maintenanceBlocks = [];

    public string Name { get; private set; } = string.Empty;
    public RoomType RoomType { get; private set; } = null!;
    public IReadOnlyCollection<RoomCapability> Capabilities => _capabilities.AsReadOnly();
    public IReadOnlyCollection<MaintenanceBlock> MaintenanceBlocks => _maintenanceBlocks.AsReadOnly();

    private Room()
    {
    }

    private Room(AuditInfo audit) : base(audit,Guid.NewGuid())
    {
    }

    public static ErrorOr<Room> Create(
        string name,
        RoomType roomType,
        List<RoomCapability>? capabilities,
        Guid createdBy,
        DateTime createdOnUtc)
    {
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(name))
        {
            errors.Add(RoomErrors.NameRequired());
        }

        if (roomType.Type != RoomCategory.Consultation
            && (capabilities is null || capabilities.Count == 0))
        {
            errors.Add(RoomErrors.NonConsultationRoomRequiresCapabilities());
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        var audit = new AuditInfo(createdBy, createdOnUtc);
        var room = new Room(audit)
        {
            Name = name,
            RoomType = roomType,
        };

        if (capabilities is not null)
        {
            room._capabilities.AddRange(capabilities);
        }

        room.RaiseDomainEvent(new RoomCreatedDomainEvent(
            room.Id, name, roomType, room._capabilities.AsReadOnly()));

        return room;
    }

    public ErrorOr<Success> ChangeType(RoomType newType, Guid updatedBy, DateTime updatedOnUtc)
    {
        var oldType = RoomType;
        RoomType = newType;

        RaiseDomainEvent(new RoomTypeChangedDomainEvent(Id, oldType, newType));
        SetUpdated(updatedBy, updatedOnUtc);

        return Result.Success;
    }

    public ErrorOr<Success> AddCapability(RoomCapability capability, Guid updatedBy, DateTime updatedOnUtc)
    {
        if (_capabilities.Any(c => c.Name.Equals(capability.Name, StringComparison.OrdinalIgnoreCase)))
        {
            return RoomErrors.DuplicateCapability(capability.Name);
        }

        _capabilities.Add(capability);

        RaiseDomainEvent(new RoomCapabilityAddedDomainEvent(Id, capability.Name));
        SetUpdated(updatedBy, updatedOnUtc);

        return Result.Success;
    }

    public ErrorOr<Success> RemoveCapability(string capabilityName, Guid updatedBy, DateTime updatedOnUtc)
    {
        var capability = _capabilities.FirstOrDefault(
            c => c.Name.Equals(capabilityName, StringComparison.OrdinalIgnoreCase));

        if (capability is null)
        {
            return RoomErrors.CapabilityNotFound(capabilityName);
        }

        _capabilities.Remove(capability);

        RaiseDomainEvent(new RoomCapabilityRemovedDomainEvent(Id, capabilityName));
        SetUpdated(updatedBy, updatedOnUtc);

        return Result.Success;
    }

    public ErrorOr<Success> ScheduleMaintenance(
        DateTime scheduledFrom,
        DateTime scheduledUntil,
        MaintenanceReason reason,
        Guid scheduledBy,
        bool overrideConfirmedAppointments,
        Guid createdBy,
        DateTime createdOnUtc)
    {
        if (scheduledUntil <= scheduledFrom)
        {
            return Error.Validation(
                "Room.MaintenanceInvalidPeriod",
                "Maintenance end time must be after start time.");
        }

        var periodResult = DateTimeRange.Create(
            new DateTimeOffset(scheduledFrom, TimeSpan.Zero),
            new DateTimeOffset(scheduledUntil, TimeSpan.Zero));

        if (periodResult.IsError)
        {
            return periodResult.Errors;
        }

        var period = periodResult.Value;

        var hasOverlap = _maintenanceBlocks.Any(b =>
            b.IsActive && b.MaintenancePeriod.OverlapsWith(period));

        if (hasOverlap)
        {
            return RoomErrors.OverlappingMaintenance();
        }

        var blockResult = MaintenanceBlock.Create(
            Id, scheduledFrom, scheduledUntil, reason, scheduledBy,
            createdBy, createdOnUtc);

        if (blockResult.IsError)
        {
            return blockResult.Errors;
        }

        _maintenanceBlocks.Add(blockResult.Value);

        RaiseDomainEvent(new MaintenanceScheduledDomainEvent(
            Id, blockResult.Value.Id, period, reason));

        SetUpdated(createdBy, createdOnUtc);

        return Result.Success;
    }

    public ErrorOr<Success> CancelMaintenance(Guid blockId, Guid cancelledBy, DateTime cancelledOnUtc)
    {
        var block = _maintenanceBlocks.FirstOrDefault(b => b.Id == blockId);

        if (block is null)
        {
            return RoomErrors.MaintenanceNotFound(blockId);
        }

        var cancelResult = block.Cancel(cancelledBy, cancelledOnUtc);
        if (cancelResult.IsError)
        {
            return cancelResult.Errors;
        }

        RaiseDomainEvent(new MaintenanceCancelledDomainEvent(Id, blockId));
        SetUpdated(cancelledBy, cancelledOnUtc);

        return Result.Success;
    }
}
