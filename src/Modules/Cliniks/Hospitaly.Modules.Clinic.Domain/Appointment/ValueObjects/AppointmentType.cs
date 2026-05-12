using ErrorOr;
using Hospitaly.Modules.Clinic.Domain.Appointment.Enums;

namespace Hospitaly.Modules.Clinic.Domain.Appointment.ValueObjects;

public sealed record AppointmentType
{
    public VisitType Type { get; }
    public TimeSpan ExpectedDuration { get; }

    private AppointmentType()
    {
    }

    private AppointmentType(VisitType type, TimeSpan expectedDuration)
    {
        Type = type;
        ExpectedDuration = expectedDuration;
    }

    public static ErrorOr<AppointmentType> Create(VisitType type, TimeSpan expectedDuration)
    {
        var errors = new List<Error>();

        if (expectedDuration <= TimeSpan.Zero)
        {
            errors.Add(Error.Validation(
                "AppointmentType.InvalidDuration",
                "Expected duration must be greater than zero."));
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return new AppointmentType(type, expectedDuration);
    }

    public override string ToString() => $"{Type} ({ExpectedDuration.TotalMinutes} min)";
}
