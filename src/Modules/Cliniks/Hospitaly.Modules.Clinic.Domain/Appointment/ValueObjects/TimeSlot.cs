using ErrorOr;
using Hospitaly.Common.Domain.Common.ValueObjects;

namespace Hospitaly.Modules.Clinic.Domain.Appointment.ValueObjects;

public sealed record TimeSlot
{
    public DateTimeRange DateTimeRange { get; }

    private TimeSlot()
    {
    }

    private TimeSlot(DateTimeRange dateTimeRange)
    {
        DateTimeRange = dateTimeRange;
    }

    public static ErrorOr<TimeSlot> Create(DateOnly date, TimeOnly startTime, TimeOnly endTime)
    {
        if (endTime <= startTime)
        {
            return Error.Validation(
                "TimeSlot.InvalidRange",
                "End time must be after start time.");
        }

        if ((endTime - startTime).TotalMinutes < 15)
        {
            return Error.Validation(
                "TimeSlot.TooShort",
                "Time slot duration must be at least 15 minutes.");
        }

        var startDt = new DateTimeOffset(date.ToDateTime(startTime, DateTimeKind.Utc), TimeSpan.Zero);
        var endDt = new DateTimeOffset(date.ToDateTime(endTime, DateTimeKind.Utc), TimeSpan.Zero);

        var dateTimeRange = DateTimeRange.Create(startDt, endDt);
        if (dateTimeRange.IsError)
        {
            return dateTimeRange.Errors;
        }

        return new TimeSlot(dateTimeRange.Value);
    }

    public DateOnly Date => DateOnly.FromDateTime(DateTimeRange.Start.DateTime);
    public TimeOnly StartTime => TimeOnly.FromDateTime(DateTimeRange.Start.DateTime);
    public TimeOnly EndTime => TimeOnly.FromDateTime((DateTimeRange.End ?? DateTimeRange.Start).DateTime);
    public TimeSpan Duration => (DateTimeRange.End ?? DateTimeRange.Start) - DateTimeRange.Start;

    public bool OverlapsWith(TimeSlot other) => DateTimeRange.OverlapsWith(other.DateTimeRange);

    public override string ToString() => $"{Date:yyyy-MM-dd} {StartTime:HH:mm}–{EndTime:HH:mm}";
}
