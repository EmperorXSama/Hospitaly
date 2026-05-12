using ErrorOr;

namespace Hospitaly.Modules.Clinic.Domain.DoctorSchedule;

public static class DoctorScheduleErrors
{
    public static Error OverlappingBlocks() =>
        Error.Conflict(
            "DoctorSchedule.OverlappingBlocks",
            "The schedule block overlaps with an existing block for the same day and time.");

    public static Error BlockNotFound(Guid blockId) =>
        Error.NotFound(
            "DoctorSchedule.BlockNotFound",
            $"The schedule block with identifier {blockId} was not found.");

    public static Error ConfirmedAppointmentsExist() =>
        Error.Conflict(
            "DoctorSchedule.ConfirmedAppointmentsExist",
            "The schedule block has confirmed appointments. Explicit override is required.");
}
