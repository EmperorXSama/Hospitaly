using ErrorOr;
using Hospitaly.Common.Domain;
using Hospitaly.Modules.Clinic.Domain.DoctorSchedule.Enums;
using Hospitaly.Modules.Clinic.Domain.DoctorSchedule.ValueObjects;

namespace Hospitaly.Modules.Clinic.Domain.DoctorSchedule;

public class DoctorSchedule : AggregateRoot
{
    private readonly List<ScheduleBlock> _blocks = [];

    public IReadOnlyCollection<ScheduleBlock> Blocks => _blocks.AsReadOnly();
    public Guid DoctorId { get; private set; }
    public Guid ClinicId { get; private set; }

    private DoctorSchedule()
    {
    }

    protected DoctorSchedule(AuditInfo audit) : base(audit,Guid.NewGuid())
    {
    }

    public static ErrorOr<DoctorSchedule> Create(
        Guid doctorId,
        Guid clinicId,
        Guid createdBy,
        DateTime createdOnUtc)
    {
        var audit = new AuditInfo(createdBy, createdOnUtc);

        return new DoctorSchedule(audit)
        {
            DoctorId = doctorId,
            ClinicId = clinicId,
        };
    }

    public ErrorOr<Success> AddBlock(
        DayOfWeek? dayOfWeek,
        DateOnly? specificDate,
        TimeOnly startTime,
        TimeOnly endTime,
        BlockType blockType,
        int maxAppointmentsAllowed,
        Guid createdBy,
        DateTime createdOnUtc)
    {
        var timeRange = TimeRange.Create(startTime, endTime);
        if (timeRange.IsError)
        {
            return timeRange.Errors;
        }

        var overlapCheck = CheckForOverlap(dayOfWeek, specificDate, timeRange.Value);
        if (overlapCheck.IsError)
        {
            return overlapCheck.Errors;
        }

        var block = ScheduleBlock.Create(
            Id,
            dayOfWeek,
            specificDate,
            timeRange.Value,
            blockType,
            maxAppointmentsAllowed,
            createdBy,
            createdOnUtc);

        if (block.IsError)
        {
            return block.Errors;
        }

        _blocks.Add(block.Value);
        return Result.Success;
    }

    public ErrorOr<Success> RemoveBlock(Guid blockId)
    {
        var block = _blocks.FirstOrDefault(b => b.Id == blockId);
        if (block is null)
        {
            return DoctorScheduleErrors.BlockNotFound(blockId);
        }

        _blocks.Remove(block);
        return Result.Success;
    }

    public ErrorOr<Success> RemoveBlock(Guid blockId, bool overrideConfirmedAppointments)
    {
        if (!overrideConfirmedAppointments)
        {
            return RemoveBlock(blockId);
        }

        var block = _blocks.FirstOrDefault(b => b.Id == blockId);
        if (block is null)
        {
            return DoctorScheduleErrors.BlockNotFound(blockId);
        }

        _blocks.Remove(block);
        return Result.Success;
    }

    public ErrorOr<Success> ChangeBlockType(
        Guid blockId,
        BlockType newType,
        Guid updatedBy,
        DateTime updatedOnUtc)
    {
        var block = _blocks.FirstOrDefault(b => b.Id == blockId);
        if (block is null)
        {
            return DoctorScheduleErrors.BlockNotFound(blockId);
        }

        return block.ChangeBlockType(newType, updatedBy, updatedOnUtc);
    }

    public ErrorOr<Success> UpdateBlockTimeRange(
        Guid blockId,
        TimeOnly newStart,
        TimeOnly newEnd,
        Guid updatedBy,
        DateTime updatedOnUtc)
    {
        var block = _blocks.FirstOrDefault(b => b.Id == blockId);
        if (block is null)
        {
            return DoctorScheduleErrors.BlockNotFound(blockId);
        }

        var timeRange = TimeRange.Create(newStart, newEnd);
        if (timeRange.IsError)
        {
            return timeRange.Errors;
        }

        var overlapCheck = CheckForOverlap(
            block.DayOfWeek,
            block.SpecificDate,
            timeRange.Value,
            block.Id);

        if (overlapCheck.IsError)
        {
            return overlapCheck.Errors;
        }

        return block.UpdateTimeRange(timeRange.Value, updatedBy, updatedOnUtc);
    }

    private ErrorOr<Success> CheckForOverlap(
        DayOfWeek? dayOfWeek,
        DateOnly? specificDate,
        TimeRange timeRange,
        Guid? excludeBlockId = null)
    {
        var hasOverlap = _blocks.Any(b =>
        {
            if (excludeBlockId.HasValue && b.Id == excludeBlockId.Value)
            {
                return false;
            }

            if (dayOfWeek.HasValue && b.DayOfWeek.HasValue && b.DayOfWeek == dayOfWeek)
            {
                return b.TimeRange.OverlapsWith(timeRange);
            }

            if (specificDate.HasValue && b.SpecificDate.HasValue && b.SpecificDate == specificDate)
            {
                return b.TimeRange.OverlapsWith(timeRange);
            }

            return false;
        });

        if (hasOverlap)
        {
            return DoctorScheduleErrors.OverlappingBlocks();
        }

        return Result.Success;
    }
}
