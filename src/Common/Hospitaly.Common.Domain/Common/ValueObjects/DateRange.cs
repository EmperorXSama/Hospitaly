using ErrorOr;

namespace Hospitaly.Common.Domain.Common.ValueObjects;


public record DateTimeRange : IEquatable<DateTimeRange>
{
    
    public DateTimeOffset Start { get; }
    public DateTimeOffset? End { get; }
    
    public bool IsOpenEnded =>  End is null;
    public bool IsClosed => End.HasValue;
    
    public bool HasDuration => IsOpenEnded || (IsClosed && Start < End.Value);
    public bool IsEnded(DateTimeOffset asOf) => asOf <= End;

    private DateTimeRange()
    {
    }

    private DateTimeRange(DateTimeOffset start, DateTimeOffset end) : this(start, (DateTimeOffset?)end)
    { }
    private DateTimeRange(DateTimeOffset start, DateTimeOffset? end = null)
    {
        Start = start;
        End = end;
    }

    public static ErrorOr<DateTimeRange> Create(DateTimeOffset start, DateTimeOffset? end)
    {
        if (end.HasValue && end.Value < start)
        {
            return Error.Validation(
                code: "DateTimeRange.Invalid",
                description: "End date cannot be earlier than start date.",
                metadata:new Dictionary<string, object>
                {
                    ["start"] = start,
                    ["end"] = end
                });
        }
        
        return new DateTimeRange(start, end);
    }

    public static ErrorOr<DateTimeRange> FromDuration(DateTimeOffset start, TimeSpan duration)
    {
        if (duration < TimeSpan.Zero)
        {
            return Error.Validation(
                code: "DateTimeRange.Invalid",
                description: "Duration cannot be negative.",
                metadata:new Dictionary<string, object>
                {
                    ["start"] = start,
                    ["duration"] = duration
                });
        }

        return new DateTimeRange(start, start + duration);
    }

    public bool OverlapsWith(DateTimeRange other)
    {
        var thisEnd = End??  DateTime.MaxValue;
        var otherEnd = other.End??  DateTime.MaxValue;
        
        return Start <= otherEnd && other.Start <= thisEnd;
    }

    public bool Contains(DateTimeOffset date)
    {
        if (date < Start)
            return false;
        return !IsClosed || date < End!.Value;
    }
    
    /// <summary>
    /// Checks if this range contains the entire other range.
    /// </summary>
    public bool ContainsRange(DateTimeRange other)
    {
        if (!Contains(other.Start) || !Contains(other.End ?? DateTimeOffset.MaxValue))
            return false;

        if (other.IsOpenEnded && IsClosed)
            return false;

        return true;
    }

    /// <summary>
    /// Gets the intersection of this range with another.
    /// Returns null if there is no overlap.
    /// </summary>
    public DateTimeRange? IntersectWith(DateTimeRange other)
    {
        if (!OverlapsWith(other))
            return null;

        var intersectStart = Start > other.Start ? Start : other.Start;
        var intersectEnd = GetIntersectedEnd(other);

        return new DateTimeRange(intersectStart, intersectEnd);
    }

    /// <summary>
    /// Gets the union of this range with another.
    /// </summary>
    public DateTimeRange GetUnionWith(DateTimeRange other)
    {
        if (!OverlapsWith(other))
        {
            throw new InvalidOperationException(
                "Cannot union non-overlapping ranges.");
        }

        var start = Start < other.Start
            ? Start
            : other.Start;

        DateTimeOffset? end;

        if (End is null || other.End is null)
        {
            end = null;
        }
        else
        {
            end = End > other.End
                ? End
                : other.End;
        }

        return new DateTimeRange(start, end);
    }
    
    
    /// <summary>
    /// Shifts the entire range by the given TimeSpan.
    /// </summary>
    public DateTimeRange Shift(TimeSpan amount)
    {
        return new DateTimeRange(Start + amount, End.HasValue ? End.Value + amount : null);
    }

    /// <summary>
    /// Shifts the entire range by the given days.
    /// </summary>
    public DateTimeRange ShiftDays(int days)
    {
        return Shift(TimeSpan.FromDays(days));
    }

    public override string ToString()
    {
        if (IsOpenEnded)
            return $"[{Start}, ∞)";

        return $"[{Start}, {End!.Value})";
    }

    private DateTimeOffset? GetIntersectedEnd(DateTimeRange other)
    {
        if (!IsClosed && !other.IsClosed) return null;
        if (!IsClosed) return other.End;
        if (!other.IsClosed) return End;

        var thisEnd = End!.Value;
        var otherEnd = other.End!.Value;
        return thisEnd < otherEnd ? thisEnd : otherEnd;
    }

    public double GetProgress(DateTimeOffset asOf)
    {
        if (asOf <= Start)
            return 0;

        if (IsOpenEnded)
            return double.NaN;

        var total = End!.Value - Start;

        if (total <= TimeSpan.Zero)
            return 1;

        var elapsed = asOf - Start;

        var progress = (double)elapsed.Ticks / total.Ticks;
        
        return Math.Clamp(progress, 0, 1);
    }
    
    
    
    
    
    
    
    
    
}