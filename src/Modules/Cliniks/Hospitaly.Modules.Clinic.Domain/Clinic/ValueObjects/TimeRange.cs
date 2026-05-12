using ErrorOr;
using System.Collections.Generic;
using Hospitaly.Common.Domain.Common.ValueObjects;

namespace Hospitaly.Modules.Clinic.Domain.Clinic.ValueObjects;

public sealed record OperatingTimeRange
{
    private bool _isActive = true;

    public DateTimeRange Value { get; init; }
    private OperatingTimeRange()
    {
    }

    private OperatingTimeRange(DateTimeRange value)
    {
        Value = value;
    }

    public static ErrorOr<OperatingTimeRange> Create(DateTimeOffset start, DateTimeOffset end)
    {
        var dateTimeRange = DateTimeRange.Create(start, end);

        if (dateTimeRange.IsError)
        {
            return dateTimeRange.Errors;
        }

        return new OperatingTimeRange(dateTimeRange.Value);
    }
}
