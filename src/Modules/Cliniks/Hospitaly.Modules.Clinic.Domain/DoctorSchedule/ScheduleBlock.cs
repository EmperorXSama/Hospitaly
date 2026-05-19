using ErrorOr;
using Hospitaly.Common.Domain;
using Hospitaly.Modules.Clinic.Domain.DoctorSchedule.Enums;
using Hospitaly.Modules.Clinic.Domain.DoctorSchedule.ValueObjects;

namespace Hospitaly.Modules.Clinic.Domain.DoctorSchedule;

public class ScheduleBlock : Entity
{
    public Guid DoctorScheduleId { get; private set; }
    public DayOfWeek? DayOfWeek { get; private set; }
    public DateOnly? SpecificDate { get; private set; }
    public TimeRange TimeRange { get; private set; }
    public bool IsRecurring => DayOfWeek.HasValue;
    public BlockType BlockType { get; private set; }
    public int MaxAppointmentsAllowed { get; private set; }

    private ScheduleBlock()
    {
    }

    private ScheduleBlock(AuditInfo audit) : base(audit,Guid.NewGuid())
    {
    }

    private ScheduleBlock(
        Guid doctorScheduleId,
        DayOfWeek? dayOfWeek,
        DateOnly? specificDate,
        TimeRange timeRange,
        BlockType blockType,
        int maxAppointmentsAllowed,
        AuditInfo audit) : base(audit,Guid.NewGuid())
    {
        DoctorScheduleId = doctorScheduleId;
        DayOfWeek = dayOfWeek;
        SpecificDate = specificDate;
        TimeRange = timeRange;
        BlockType = blockType;
        MaxAppointmentsAllowed = maxAppointmentsAllowed;
    }

    public static ErrorOr<ScheduleBlock> Create(
        Guid doctorScheduleId,
        DayOfWeek? dayOfWeek,
        DateOnly? specificDate,
        TimeRange timeRange,
        BlockType blockType,
        int maxAppointmentsAllowed,
        Guid createdBy,
        DateTime createdOnUtc)
    {
        var errors = new List<Error>();

        if (dayOfWeek.HasValue == specificDate.HasValue)
        {
            errors.Add(Error.Validation(
                "ScheduleBlock.InvalidDay",
                "Exactly one of DayOfWeek or SpecificDate must be specified."));
        }

        if (maxAppointmentsAllowed <= 0)
        {
            errors.Add(Error.Validation(
                "ScheduleBlock.InvalidMaxAppointments",
                "MaxAppointmentsAllowed must be greater than zero."));
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        var audit = new AuditInfo(createdBy, createdOnUtc);

        return new ScheduleBlock(
            doctorScheduleId,
            dayOfWeek,
            specificDate,
            timeRange,
            blockType,
            maxAppointmentsAllowed,
            audit);
    }

    public ErrorOr<Success> ChangeBlockType(BlockType newType, Guid updatedBy, DateTime updatedOnUtc)
    {
        BlockType = newType;
        SetUpdated(updatedBy, updatedOnUtc);
        return Result.Success;
    }

    public ErrorOr<Success> UpdateTimeRange(TimeRange newTimeRange, Guid updatedBy, DateTime updatedOnUtc)
    {
        TimeRange = newTimeRange;
        SetUpdated(updatedBy, updatedOnUtc);
        return Result.Success;
    }
}
