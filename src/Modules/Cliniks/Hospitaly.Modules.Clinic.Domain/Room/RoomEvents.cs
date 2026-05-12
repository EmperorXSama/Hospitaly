using Hospitaly.Common.Domain;
using Hospitaly.Common.Domain.Common.ValueObjects;
using Hospitaly.Modules.Clinic.Domain.Room.Enums;
using Hospitaly.Modules.Clinic.Domain.Room.ValueObjects;

namespace Hospitaly.Modules.Clinic.Domain.Room;

public sealed class RoomCreatedDomainEvent : DomainEvent
{
    public Guid RoomId { get; }
    public string Name { get; }
    public RoomType RoomType { get; }
    public IReadOnlyCollection<RoomCapability> Capabilities { get; }

    public RoomCreatedDomainEvent(
        Guid roomId,
        string name,
        RoomType roomType,
        IReadOnlyCollection<RoomCapability> capabilities)
    {
        RoomId = roomId;
        Name = name;
        RoomType = roomType;
        Capabilities = capabilities;
    }
}

public sealed class RoomTypeChangedDomainEvent : DomainEvent
{
    public Guid RoomId { get; }
    public RoomType OldType { get; }
    public RoomType NewType { get; }

    public RoomTypeChangedDomainEvent(Guid roomId, RoomType oldType, RoomType newType)
    {
        RoomId = roomId;
        OldType = oldType;
        NewType = newType;
    }
}

public sealed class RoomCapabilityAddedDomainEvent : DomainEvent
{
    public Guid RoomId { get; }
    public string CapabilityName { get; }

    public RoomCapabilityAddedDomainEvent(Guid roomId, string capabilityName)
    {
        RoomId = roomId;
        CapabilityName = capabilityName;
    }
}

public sealed class RoomCapabilityRemovedDomainEvent : DomainEvent
{
    public Guid RoomId { get; }
    public string CapabilityName { get; }

    public RoomCapabilityRemovedDomainEvent(Guid roomId, string capabilityName)
    {
        RoomId = roomId;
        CapabilityName = capabilityName;
    }
}

public sealed class MaintenanceScheduledDomainEvent : DomainEvent
{
    public Guid RoomId { get; }
    public Guid MaintenanceBlockId { get; }
    public DateTimeRange Period { get; }
    public MaintenanceReason Reason { get; }

    public MaintenanceScheduledDomainEvent(
        Guid roomId,
        Guid maintenanceBlockId,
        DateTimeRange period,
        MaintenanceReason reason)
    {
        RoomId = roomId;
        MaintenanceBlockId = maintenanceBlockId;
        Period = period;
        Reason = reason;
    }
}

public sealed class MaintenanceCancelledDomainEvent : DomainEvent
{
    public Guid RoomId { get; }
    public Guid MaintenanceBlockId { get; }

    public MaintenanceCancelledDomainEvent(Guid roomId, Guid maintenanceBlockId)
    {
        RoomId = roomId;
        MaintenanceBlockId = maintenanceBlockId;
    }
}

public sealed class MaintenanceExtendedDomainEvent : DomainEvent
{
    public Guid RoomId { get; }
    public Guid MaintenanceBlockId { get; }
    public DateTime OldUntil { get; }
    public DateTime NewUntil { get; }

    public MaintenanceExtendedDomainEvent(
        Guid roomId,
        Guid maintenanceBlockId,
        DateTime oldUntil,
        DateTime newUntil)
    {
        RoomId = roomId;
        MaintenanceBlockId = maintenanceBlockId;
        OldUntil = oldUntil;
        NewUntil = newUntil;
    }
}
