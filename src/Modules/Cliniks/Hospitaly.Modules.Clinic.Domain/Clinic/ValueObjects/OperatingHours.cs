using ErrorOr;
using System.Collections.Generic;
using System.Linq;

namespace Hospitaly.Modules.Clinic.Domain.Clinic.ValueObjects;

public sealed record OperatingHours
{
    public DayOfWeek Day { get; init; }
    public OperatingTimeRange? Hours { get; init; }
    public bool IsResting { get; init; }
    public bool IsOffDay => Hours is null;

    private OperatingHours()
    {
    }

    private OperatingHours(DayOfWeek day, OperatingTimeRange? hours, bool isResting)
    {
        Day = day;
        Hours = hours;
        IsResting = isResting;
    }

    public static ErrorOr<OperatingHours> Create(
        DayOfWeek day,
        bool isClosed,
        TimeSpan? openTime = null,
        TimeSpan? closeTime = null,
        bool isResting = false)
    {
        var errors = new List<Error>();

        OperatingTimeRange? hours = null;
        
        if (!isClosed)
        {
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
                var startDto = baseDate + openTime.Value;
                var endDto = baseDate + closeTime.Value;
                var h = OperatingTimeRange.Create(startDto, endDto);
                if (h.IsError)
                {
                    errors.AddRange(h.Errors);
                }
                else
                {
                    hours = h.Value;
                }
            }
        }

        if (errors.Any())
        {
            return errors;
        }

        return new OperatingHours(day, hours, isResting);
    }
}
