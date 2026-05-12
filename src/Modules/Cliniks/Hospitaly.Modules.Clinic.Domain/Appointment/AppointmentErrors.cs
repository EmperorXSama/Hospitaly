using ErrorOr;
using Hospitaly.Modules.Clinic.Domain.Appointment.ValueObjects;

namespace Hospitaly.Modules.Clinic.Domain.Appointment;

public static class AppointmentErrors
{
    public static Error StartTimeMustBeInFuture() =>
        Error.Validation(
            "Appointment.StartTimeMustBeInFuture",
            "The appointment start time must be in the future.");

    public static Error DurationDoesNotMatchType() =>
        Error.Validation(
            "Appointment.DurationDoesNotMatchType",
            "The time slot duration must match the appointment type's expected duration.");

    public static Error InvalidStatusTransition(AppointmentStatusValue from, AppointmentStatusValue to) =>
        Error.Conflict(
            "Appointment.InvalidStatusTransition",
            $"Cannot transition from {from} to {to}.");

    public static Error AppointmentInTerminalState() =>
        Error.Conflict(
            "Appointment.InTerminalState",
            "Cannot modify an appointment that is in a terminal state (Completed, Cancelled, or NoShow).");

    public static Error NotConfirmed() =>
        Error.Validation(
            "Appointment.NotConfirmed",
            "The appointment must be confirmed before this action.");

    public static Error NotCheckedIn() =>
        Error.Validation(
            "Appointment.NotCheckedIn",
            "The appointment must be checked in before starting.");

    public static Error NotInProgress() =>
        Error.Validation(
            "Appointment.NotInProgress",
            "The appointment must be in progress before completing.");

    public static Error NewTimeSlotMustDiffer() =>
        Error.Validation(
            "Appointment.NewTimeSlotMustDiffer",
            "The new time slot must differ from the current time slot.");

    public static Error AppointmentNotFound(Guid appointmentId) =>
        Error.NotFound(
            "Appointment.NotFound",
            $"The appointment with identifier {appointmentId} was not found.");
}
