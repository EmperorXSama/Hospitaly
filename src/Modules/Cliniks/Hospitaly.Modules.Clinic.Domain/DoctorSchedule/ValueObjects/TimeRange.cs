using ErrorOr;
using Hospitaly.Common.Domain.Common.ValueObjects;

namespace Hospitaly.Modules.Clinic.Domain.DoctorSchedule.ValueObjects;

public sealed record TimeRange
{
    private static readonly DateTimeOffset Epoch = new(1, 1, 1, 0, 0, 0, TimeSpan.Zero);

    public DateTimeRange Value { get; init; }

    private TimeRange()
    {
    }

    private TimeRange(DateTimeRange value)
    {
        Value = value;
    }

    public static ErrorOr<TimeRange> Create(TimeOnly start, TimeOnly end)
    {
        if (end <= start)
        {
            return Error.Validation(
                "TimeRange.Invalid",
                "End time must be after start time.");
        }

        if ((end - start).TotalMinutes < 15)
        {
            return Error.Validation(
                "TimeRange.TooShort",
                "Duration must be at least 15 minutes.");
        }

        var startDt = Epoch + start.ToTimeSpan();
        var endDt = Epoch + end.ToTimeSpan();

        var dateTimeRange = DateTimeRange.Create(startDt, endDt);
        if (dateTimeRange.IsError)
        {
            return dateTimeRange.Errors;
        }

        return new TimeRange(dateTimeRange.Value);
    }

    public TimeOnly StartTime => TimeOnly.FromDateTime(Value.Start.DateTime);

    public TimeOnly EndTime => TimeOnly.FromDateTime((Value.End ?? Value.Start).DateTime);

    public TimeSpan Duration => (Value.End ?? Value.Start) - Value.Start;

    public bool OverlapsWith(TimeRange other) => Value.OverlapsWith(other.Value);
}
