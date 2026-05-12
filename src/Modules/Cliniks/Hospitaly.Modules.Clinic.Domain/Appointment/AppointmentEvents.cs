using Hospitaly.Common.Domain;
using Hospitaly.Modules.Clinic.Domain.Appointment.Enums;
using Hospitaly.Modules.Clinic.Domain.Appointment.ValueObjects;

namespace Hospitaly.Modules.Clinic.Domain.Appointment;

public sealed class AppointmentRequestedDomainEvent : DomainEvent
{
    public Guid AppointmentId { get; }
    public Guid DoctorId { get; }
    public Guid PatientId { get; }
    public Guid ClinicId { get; }
    public Guid? RoomId { get; }
    public TimeSlot TimeSlot { get; }
    public AppointmentType AppointmentType { get; }

    public AppointmentRequestedDomainEvent(
        Guid appointmentId,
        Guid doctorId,
        Guid patientId,
        Guid clinicId,
        Guid? roomId,
        TimeSlot timeSlot,
        AppointmentType appointmentType)
    {
        AppointmentId = appointmentId;
        DoctorId = doctorId;
        PatientId = patientId;
        ClinicId = clinicId;
        RoomId = roomId;
        TimeSlot = timeSlot;
        AppointmentType = appointmentType;
    }
}

public sealed class AppointmentConfirmedDomainEvent : DomainEvent
{
    public Guid AppointmentId { get; }
    public Guid ConfirmedBy { get; }

    public AppointmentConfirmedDomainEvent(Guid appointmentId, Guid confirmedBy)
    {
        AppointmentId = appointmentId;
        ConfirmedBy = confirmedBy;
    }
}

public sealed class AppointmentCheckedInDomainEvent : DomainEvent
{
    public Guid AppointmentId { get; }
    public DateTime CheckedInAt { get; }

    public AppointmentCheckedInDomainEvent(Guid appointmentId, DateTime checkedInAt)
    {
        AppointmentId = appointmentId;
        CheckedInAt = checkedInAt;
    }
}

public sealed class AppointmentStartedDomainEvent : DomainEvent
{
    public Guid AppointmentId { get; }
    public Guid StartedBy { get; }

    public AppointmentStartedDomainEvent(Guid appointmentId, Guid startedBy)
    {
        AppointmentId = appointmentId;
        StartedBy = startedBy;
    }
}

public sealed class AppointmentCompletedDomainEvent : DomainEvent
{
    public Guid AppointmentId { get; }
    public Guid CompletedBy { get; }

    public AppointmentCompletedDomainEvent(Guid appointmentId, Guid completedBy)
    {
        AppointmentId = appointmentId;
        CompletedBy = completedBy;
    }
}

public sealed class AppointmentCancelledDomainEvent : DomainEvent
{
    public Guid AppointmentId { get; }
    public CancellationDetails Cancellation { get; }
    public Guid CancelledBy { get; }

    public AppointmentCancelledDomainEvent(
        Guid appointmentId,
        CancellationDetails cancellation,
        Guid cancelledBy)
    {
        AppointmentId = appointmentId;
        Cancellation = cancellation;
        CancelledBy = cancelledBy;
    }
}

public sealed class AppointmentNoShowDomainEvent : DomainEvent
{
    public Guid AppointmentId { get; }
    public Guid MarkedBy { get; }

    public AppointmentNoShowDomainEvent(Guid appointmentId, Guid markedBy)
    {
        AppointmentId = appointmentId;
        MarkedBy = markedBy;
    }
}

public sealed class AppointmentRescheduledDomainEvent : DomainEvent
{
    public Guid AppointmentId { get; }
    public TimeSlot OriginalTimeSlot { get; }
    public TimeSlot NewTimeSlot { get; }
    public string Reason { get; }
    public RescheduleRequestedBy RequestedBy { get; }

    public AppointmentRescheduledDomainEvent(
        Guid appointmentId,
        TimeSlot originalTimeSlot,
        TimeSlot newTimeSlot,
        string reason,
        RescheduleRequestedBy requestedBy)
    {
        AppointmentId = appointmentId;
        OriginalTimeSlot = originalTimeSlot;
        NewTimeSlot = newTimeSlot;
        Reason = reason;
        RequestedBy = requestedBy;
    }
}
