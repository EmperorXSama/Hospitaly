using ErrorOr;
using Hospitaly.Common.Domain;
using Hospitaly.Common.Domain.Common.ValueObjects;
using Hospitaly.Modules.Clinic.Domain.Room.Enums;

namespace Hospitaly.Modules.Clinic.Domain.Room;

public class MaintenanceBlock : Entity
{
    public Guid RoomId { get; private set; }
    public DateTimeRange MaintenancePeriod { get; private set; } = null!;
    public MaintenanceReason Reason { get; private set; }
    public Guid ScheduledBy { get; private set; }
    public DateTime? CancelledAt { get; private set; }
    public bool IsActive => !CancelledAt.HasValue;

    private MaintenanceBlock()
    {
    }

    private MaintenanceBlock(AuditInfo audit) : base(audit)
    {
    }

    private MaintenanceBlock(
        Guid roomId,
        DateTimeRange maintenancePeriod,
        MaintenanceReason reason,
        Guid scheduledBy,
        AuditInfo audit) : base(audit)
    {
        RoomId = roomId;
        MaintenancePeriod = maintenancePeriod;
        Reason = reason;
        ScheduledBy = scheduledBy;
    }

    public static ErrorOr<MaintenanceBlock> Create(
        Guid roomId,
        DateTime scheduledFrom,
        DateTime scheduledUntil,
        MaintenanceReason reason,
        Guid scheduledBy,
        Guid createdBy,
        DateTime createdOnUtc)
    {
        if (scheduledUntil <= scheduledFrom)
        {
            return Error.Validation(
                "MaintenanceBlock.InvalidPeriod",
                "Maintenance end time must be after start time.");
        }

        var period = DateTimeRange.Create(
            new DateTimeOffset(scheduledFrom, TimeSpan.Zero),
            new DateTimeOffset(scheduledUntil, TimeSpan.Zero));

        if (period.IsError)
        {
            return period.Errors;
        }

        var audit = new AuditInfo(createdBy, createdOnUtc);

        return new MaintenanceBlock(roomId, period.Value, reason, scheduledBy, audit);
    }

    public ErrorOr<Success> Extend(DateTime newUntil, Guid updatedBy, DateTime updatedOnUtc)
    {
        if (!IsActive)
        {
            return RoomErrors.MaintenanceAlreadyCancelled();
        }

        if (newUntil <= MaintenancePeriod.End!.Value.DateTime)
        {
            return RoomErrors.ExtendMustBeLater();
        }

        var newPeriod = DateTimeRange.Create(MaintenancePeriod.Start, new DateTimeOffset(newUntil, TimeSpan.Zero));
        if (newPeriod.IsError)
        {
            return newPeriod.Errors;
        }

        MaintenancePeriod = newPeriod.Value;
        SetUpdated(updatedBy, updatedOnUtc);

        return Result.Success;
    }

    public ErrorOr<Success> Cancel(Guid cancelledBy, DateTime cancelledOnUtc)
    {
        if (!IsActive)
        {
            return RoomErrors.MaintenanceAlreadyCancelled();
        }

        CancelledAt = cancelledOnUtc;
        SetUpdated(cancelledBy, cancelledOnUtc);

        return Result.Success;
    }
}
