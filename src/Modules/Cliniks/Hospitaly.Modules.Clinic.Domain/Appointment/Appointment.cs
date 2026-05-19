using ErrorOr;
using Hospitaly.Common.Domain;
using Hospitaly.Modules.Clinic.Domain.Appointment.Enums;
using Hospitaly.Modules.Clinic.Domain.Appointment.ValueObjects;

namespace Hospitaly.Modules.Clinic.Domain.Appointment;

public class Appointment : AggregateRoot
{
    private static readonly Dictionary<AppointmentStatusValue, HashSet<AppointmentStatusValue>> AllowedTransitions = new()
    {
        [AppointmentStatusValue.Requested] = [AppointmentStatusValue.Confirmed, AppointmentStatusValue.Cancelled],
        [AppointmentStatusValue.Confirmed] = [AppointmentStatusValue.CheckedIn, AppointmentStatusValue.Cancelled, AppointmentStatusValue.Rescheduled],
        [AppointmentStatusValue.CheckedIn] = [AppointmentStatusValue.InProgress, AppointmentStatusValue.Cancelled, AppointmentStatusValue.NoShow],
        [AppointmentStatusValue.InProgress] = [AppointmentStatusValue.Completed, AppointmentStatusValue.NoShow],
        [AppointmentStatusValue.Rescheduled] = [AppointmentStatusValue.Confirmed, AppointmentStatusValue.Cancelled],
        [AppointmentStatusValue.Completed] = [],
        [AppointmentStatusValue.Cancelled] = [],
        [AppointmentStatusValue.NoShow] = [],
    };

    public Guid DoctorId { get; private set; }
    public Guid PatientId { get; private set; }
    public Guid ClinicId { get; private set; }
    public Guid? RoomId { get; private set; }

    public TimeSlot TimeSlot { get; private set; } = null!;
    public AppointmentType AppointmentType { get; private set; } = null!;
    public AppointmentStatus Status { get; private set; } = null!;
    public CancellationDetails? Cancellation { get; private set; }
    public RescheduleInfo? RescheduleInfo { get; private set; }

    private Appointment()
    {
    }

    private Appointment(AuditInfo audit) : base(audit,Guid.NewGuid())
    {
    }

    public static ErrorOr<Appointment> Request(
        Guid doctorId,
        Guid patientId,
        Guid clinicId,
        Guid? roomId,
        TimeSlot timeSlot,
        AppointmentType appointmentType,
        Guid requestedBy,
        DateTime requestedOnUtc)
    {
        var errors = new List<Error>();

        if (timeSlot.DateTimeRange.Start <= DateTimeOffset.UtcNow)
        {
            errors.Add(AppointmentErrors.StartTimeMustBeInFuture());
        }

        if (timeSlot.Duration != appointmentType.ExpectedDuration)
        {
            errors.Add(AppointmentErrors.DurationDoesNotMatchType());
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        var audit = new AuditInfo(requestedBy, requestedOnUtc);
        var statusResult = AppointmentStatus.Create(AppointmentStatusValue.Requested, requestedOnUtc);
        if (statusResult.IsError)
        {
            return statusResult.Errors;
        }

        var appointment = new Appointment(audit)
        {
            DoctorId = doctorId,
            PatientId = patientId,
            ClinicId = clinicId,
            RoomId = roomId,
            TimeSlot = timeSlot,
            AppointmentType = appointmentType,
            Status = statusResult.Value,
        };

        appointment.RaiseDomainEvent(new AppointmentRequestedDomainEvent(
            appointment.Id, doctorId, patientId, clinicId, roomId, timeSlot, appointmentType));

        return appointment;
    }

    public ErrorOr<Success> Confirm(Guid confirmedBy, DateTime confirmedOnUtc)
    {
        if (Status.IsTerminal)
        {
            return AppointmentErrors.AppointmentInTerminalState();
        }

        var transition = TransitionTo(AppointmentStatusValue.Confirmed, confirmedOnUtc);
        if (transition.IsError)
        {
            return transition.Errors;
        }

        RaiseDomainEvent(new AppointmentConfirmedDomainEvent(Id, confirmedBy));
        SetUpdated(confirmedBy, confirmedOnUtc);

        return Result.Success;
    }

    public ErrorOr<Success> CheckIn(Guid checkedInBy, DateTime checkedInAt)
    {
        if (Status.IsTerminal)
        {
            return AppointmentErrors.AppointmentInTerminalState();
        }

        var transition = TransitionTo(AppointmentStatusValue.CheckedIn, checkedInAt);
        if (transition.IsError)
        {
            return transition.Errors;
        }

        RaiseDomainEvent(new AppointmentCheckedInDomainEvent(Id, checkedInAt));
        SetUpdated(checkedInBy, checkedInAt);

        return Result.Success;
    }

    public ErrorOr<Success> Start(Guid startedBy, DateTime startedAt)
    {
        if (Status.IsTerminal)
        {
            return AppointmentErrors.AppointmentInTerminalState();
        }

        var transition = TransitionTo(AppointmentStatusValue.InProgress, startedAt);
        if (transition.IsError)
        {
            return transition.Errors;
        }

        RaiseDomainEvent(new AppointmentStartedDomainEvent(Id, startedBy));
        SetUpdated(startedBy, startedAt);

        return Result.Success;
    }

    public ErrorOr<Success> Complete(Guid completedBy, DateTime completedAt)
    {
        if (Status.IsTerminal)
        {
            return AppointmentErrors.AppointmentInTerminalState();
        }

        var transition = TransitionTo(AppointmentStatusValue.Completed, completedAt);
        if (transition.IsError)
        {
            return transition.Errors;
        }

        RaiseDomainEvent(new AppointmentCompletedDomainEvent(Id, completedBy));
        SetUpdated(completedBy, completedAt);

        return Result.Success;
    }

    public ErrorOr<Success> Cancel(
        CancellationDetails cancellation,
        Guid cancelledBy,
        DateTime cancelledOnUtc)
    {
        if (Status.IsTerminal)
        {
            return AppointmentErrors.AppointmentInTerminalState();
        }

        var transition = TransitionTo(AppointmentStatusValue.Cancelled, cancelledOnUtc);
        if (transition.IsError)
        {
            return transition.Errors;
        }

        Cancellation = cancellation;

        RaiseDomainEvent(new AppointmentCancelledDomainEvent(Id, cancellation, cancelledBy));
        SetUpdated(cancelledBy, cancelledOnUtc);

        return Result.Success;
    }

    public ErrorOr<Success> MarkNoShow(Guid markedBy, DateTime noShowAt)
    {
        if (Status.IsTerminal)
        {
            return AppointmentErrors.AppointmentInTerminalState();
        }

        var transition = TransitionTo(AppointmentStatusValue.NoShow, noShowAt);
        if (transition.IsError)
        {
            return transition.Errors;
        }

        RaiseDomainEvent(new AppointmentNoShowDomainEvent(Id, markedBy));
        SetUpdated(markedBy, noShowAt);

        return Result.Success;
    }

    public ErrorOr<Success> Reschedule(
        TimeSlot newTimeSlot,
        string reason,
        RescheduleRequestedBy requestedBy,
        Guid rescheduledBy,
        DateTime rescheduledAt)
    {
        if (Status.IsTerminal)
        {
            return AppointmentErrors.AppointmentInTerminalState();
        }

        if (Status.Status != AppointmentStatusValue.Confirmed)
        {
            return AppointmentErrors.InvalidStatusTransition(Status.Status, AppointmentStatusValue.Rescheduled);
        }

        if (newTimeSlot.DateTimeRange.Start == TimeSlot.DateTimeRange.Start &&
            newTimeSlot.DateTimeRange.End == TimeSlot.DateTimeRange.End)
        {
            return AppointmentErrors.NewTimeSlotMustDiffer();
        }

        var originalTimeSlot = TimeSlot;

        TimeSlot = newTimeSlot;

        var rescheduleInfo = RescheduleInfo.Create(originalTimeSlot, reason, rescheduledAt, requestedBy);
        if (rescheduleInfo.IsError)
        {
            return rescheduleInfo.Errors;
        }

        RescheduleInfo = rescheduleInfo.Value;

        var transition = TransitionTo(AppointmentStatusValue.Rescheduled, rescheduledAt);
        if (transition.IsError)
        {
            return transition.Errors;
        }

        RaiseDomainEvent(new AppointmentRescheduledDomainEvent(
            Id, originalTimeSlot, newTimeSlot, reason, requestedBy));

        transition = TransitionTo(AppointmentStatusValue.Confirmed, rescheduledAt);
        if (transition.IsError)
        {
            return transition.Errors;
        }

        SetUpdated(rescheduledBy, rescheduledAt);

        return Result.Success;
    }

    private ErrorOr<Success> TransitionTo(AppointmentStatusValue newStatus, DateTime setAt)
    {
        if (!AllowedTransitions.TryGetValue(Status.Status, out var allowed) || !allowed.Contains(newStatus))
        {
            return AppointmentErrors.InvalidStatusTransition(Status.Status, newStatus);
        }

        var statusResult = AppointmentStatus.Create(newStatus, setAt);
        if (statusResult.IsError)
        {
            return statusResult.Errors;
        }

        Status = statusResult.Value;
        return Result.Success;
    }
}
