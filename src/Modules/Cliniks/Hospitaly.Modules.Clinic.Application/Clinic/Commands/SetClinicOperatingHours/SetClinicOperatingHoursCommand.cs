using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.SetClinicOperatingHours;

public sealed record SetClinicOperatingHoursCommand(
    Guid ClinicId,
    Guid UserId,
    List<OperatingHoursDto> OperatingHours
    ): ICommand<ErrorOr.Success>;
public record OperatingHoursDto(
    DayOfWeek Day,
    bool IsClosed,         
    TimeSpan? StartTime,
    TimeSpan? EndTime,
    TimeSpan? RestingStartsAt,
    TimeSpan? RestingEndsAt);