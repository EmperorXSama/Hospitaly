using ErrorOr;
using Hospitaly.Common.Domain.Common.ValueObjects;
using Hospitaly.Modules.Clinic.Domain.Clinic.Enum;

namespace Hospitaly.Modules.Clinic.Domain.Clinic.ValueObjects;

public record LicenceValidityPeriod
{
    
    public DateTimeRange Value { get; init; }

    private LicenceValidityPeriod()
    {
    }

    private LicenceValidityPeriod(DateTimeRange value)
    {
        Value = value;
    }

    public static ErrorOr<LicenceValidityPeriod> Create(DateTimeOffset start, DateTimeOffset end)
    {
        var period =  DateTimeRange.Create(start, end);
        if (period.IsError)
        {
            return period.Errors;
        }

        return new LicenceValidityPeriod(period.Value);
    }
    
    public bool IsExpired(DateTimeOffset asOf) => !Value.Contains(asOf);
    public LicenceValidityStatus GetStatus(DateTimeOffset asOf)
    {
        if (asOf < Value.Start)
            return LicenceValidityStatus.NotStarted;

        if (IsExpired(asOf))
            return LicenceValidityStatus.Expired;

        var progress = Value.GetProgress(asOf);

        if (progress >= 0.9)
            return LicenceValidityStatus.ExpiringSoon;

        return LicenceValidityStatus.Active;
    }
}