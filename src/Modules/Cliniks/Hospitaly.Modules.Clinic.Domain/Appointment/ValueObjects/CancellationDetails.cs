using ErrorOr;
using Hospitaly.Modules.Clinic.Domain.Appointment.Enums;

namespace Hospitaly.Modules.Clinic.Domain.Appointment.ValueObjects;

public sealed record CancellationDetails
{
    public CancellationReason Reason { get; }
    public InitiatedBy InitiatedBy { get; }
    public string? Notes { get; }
    public DateTime CancelledAt { get; }

    private CancellationDetails()
    {
    }

    private CancellationDetails(
        CancellationReason reason,
        InitiatedBy initiatedBy,
        DateTime cancelledAt,
        string? notes)
    {
        Reason = reason;
        InitiatedBy = initiatedBy;
        CancelledAt = cancelledAt;
        Notes = notes;
    }

    public static ErrorOr<CancellationDetails> Create(
        CancellationReason reason,
        InitiatedBy initiatedBy,
        DateTime cancelledAt,
        string? notes = null)
    {
        return new CancellationDetails(reason, initiatedBy, cancelledAt, notes);
    }

    public override string ToString() =>
        $"Cancelled: {Reason} (by {InitiatedBy}) at {CancelledAt:O}";
}
