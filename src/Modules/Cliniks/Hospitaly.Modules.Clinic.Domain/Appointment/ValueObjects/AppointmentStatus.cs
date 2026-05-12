using ErrorOr;

namespace Hospitaly.Modules.Clinic.Domain.Appointment.ValueObjects;

public sealed record AppointmentStatus
{
    public AppointmentStatusValue Status { get; }
    public DateTime SetAt { get; }

    private AppointmentStatus()
    {
    }

    private AppointmentStatus(AppointmentStatusValue status, DateTime setAt)
    {
        Status = status;
        SetAt = setAt;
    }

    public static ErrorOr<AppointmentStatus> Create(AppointmentStatusValue status, DateTime setAt)
    {
        return new AppointmentStatus(status, setAt);
    }

    public bool IsTerminal => Status is AppointmentStatusValue.Completed
        or AppointmentStatusValue.Cancelled
        or AppointmentStatusValue.NoShow;

    public override string ToString() => $"{Status} @ {SetAt:O}";
}

public enum AppointmentStatusValue
{
    Requested,
    Confirmed,
    CheckedIn,
    InProgress,
    Completed,
    Cancelled,
    NoShow,
    Rescheduled
}
