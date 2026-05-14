using ErrorOr;
using System.Collections.Generic;
using System.Linq;

namespace Hospitaly.Modules.Clinic.Domain.Clinic.ValueObjects;

public sealed record OperatingHours
{
    public DayOfWeek Day { get; init; }
    public OperatingTimeRange? Hours { get; init; }
    public OperatingTimeRange? RestingTime { get; init; }
    
    public bool IsOffDay => Hours is null;
    public bool HasRestingTime => RestingTime is not null;

    private OperatingHours()
    {
    }

    private OperatingHours(
        DayOfWeek day, 
        OperatingTimeRange? hours,
        OperatingTimeRange? restingTime )
    {
        Day = day;
        Hours = hours;
        RestingTime = restingTime;
    }

    public static ErrorOr<OperatingHours> Create(
        DayOfWeek day,
        bool isClosed,
        TimeSpan? openTime = null,
        TimeSpan? closeTime = null,
        TimeSpan? restingStartTime = null,
        TimeSpan? restingEndTime = null)
    {
        var errors = new List<Error>();

        OperatingTimeRange? hours = null;
        OperatingTimeRange? restingTime = null;
        
       if (isClosed)
        {
            if (openTime.HasValue || closeTime.HasValue || restingStartTime.HasValue || restingEndTime.HasValue)
            {
                errors.Add(Error.Validation(
                    code: "OperatingHours.ClosedDayCannotHaveTimes",
                    description: "Closed days cannot have operating or resting times.",
                    metadata: new Dictionary<string, object> { ["day"] = day }));
            }

            return errors.Any()
                ? errors
                : new OperatingHours(day, null, null);
        }

        var baseDate = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);

        if (!openTime.HasValue || !closeTime.HasValue)
        {
            errors.Add(Error.Validation(
                code: "OperatingHours.MissingTimes",
                description: "Open and close times must be provided for non-closed days.",
                metadata: new Dictionary<string, object> { ["day"] = day }));
        }
        else
        {
            var operatingRangeResult = OperatingTimeRange.Create(
                baseDate + openTime.Value,
                baseDate + closeTime.Value);

            if (operatingRangeResult.IsError)
            {
                errors.AddRange(operatingRangeResult.Errors);
            }
            else
            {
                hours = operatingRangeResult.Value;
            }
        }

        var hasAnyRestingTime =
            restingStartTime.HasValue || restingEndTime.HasValue;

        if (hasAnyRestingTime)
        {
            if (!restingStartTime.HasValue || !restingEndTime.HasValue)
            {
                errors.Add(Error.Validation(
                    code: "OperatingHours.InvalidRestingTime",
                    description: "Both resting start time and resting end time must be provided.",
                    metadata: new Dictionary<string, object> { ["day"] = day }));
            }
            else
            {
                var restingRangeResult = OperatingTimeRange.Create(
                    baseDate + restingStartTime.Value,
                    baseDate + restingEndTime.Value);

                if (restingRangeResult.IsError)
                {
                    errors.AddRange(restingRangeResult.Errors);
                }
                else
                {
                    restingTime = restingRangeResult.Value;
                }
            }
        }

        if (hours is not null && restingTime is not null)
        {
            if (!hours.Value.ContainsRange(restingTime.Value))
            {
                errors.Add(Error.Validation(
                    code: "OperatingHours.RestingTimeOutsideOperatingHours",
                    description: "Resting time must be inside operating hours.",
                    metadata: new Dictionary<string, object>
                    {
                        ["day"] = day,
                        ["openTime"] = openTime!,
                        ["closeTime"] = closeTime!,
                        ["restingStartTime"] = restingStartTime!,
                        ["restingEndTime"] = restingEndTime!
                    }));
            }
        }

        if (errors.Any())
        {
            return errors;
        }

        return new OperatingHours(day, hours, restingTime);
    }
}
