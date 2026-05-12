using ErrorOr;
using Hospitaly.Modules.Clinic.Domain.Appointment.Enums;

namespace Hospitaly.Modules.Clinic.Domain.Appointment.ValueObjects;

public sealed record RescheduleInfo
{
    public TimeSlot OriginalTimeSlot { get; }
    public string Reason { get; }
    public DateTime RescheduledAt { get; }
    public RescheduleRequestedBy RequestedBy { get; }

    private RescheduleInfo()
    {
    }

    private RescheduleInfo(
        TimeSlot originalTimeSlot,
        string reason,
        DateTime rescheduledAt,
        RescheduleRequestedBy requestedBy)
    {
        OriginalTimeSlot = originalTimeSlot;
        Reason = reason;
        RescheduledAt = rescheduledAt;
        RequestedBy = requestedBy;
    }

    public static ErrorOr<RescheduleInfo> Create(
        TimeSlot originalTimeSlot,
        string reason,
        DateTime rescheduledAt,
        RescheduleRequestedBy requestedBy)
    {
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(reason))
        {
            errors.Add(Error.Validation(
                "RescheduleInfo.ReasonRequired",
                "Reschedule reason is required."));
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return new RescheduleInfo(originalTimeSlot, reason, rescheduledAt, requestedBy);
    }

    public override string ToString() =>
        $"Rescheduled from {OriginalTimeSlot}: {Reason} (by {RequestedBy})";
}
