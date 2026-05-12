using Hospitaly.Common.Domain.Common.ValueObjects;
using ErrorOr;
namespace Hospitaly.Modules.Clinic.Domain.Clinic.ValueObjects;

public sealed record OwnershipEffectiveRange 
{
    public DateTimeRange Range { get; init; }
    private OwnershipEffectiveRange()
    { }
    public OwnershipEffectiveRange(DateTimeRange value)

    {
        Range = value;
    }
}