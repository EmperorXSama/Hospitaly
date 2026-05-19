namespace Hospitaly.Modules.Clinic.Application.Clinic.Queries.GetClinicOperatingHours;

public record ClinicOperatingHoursResponse(
    Guid ClinicId,
    List<OperatingHoursDto> OperatingHours
);

public record OperatingHoursDto
{
    public string Day { get; init; }
    public bool HoursActive { get; init; }
    public DateTime? OpenTime { get; init; }
    public DateTime? CloseTime { get; init; }
    public bool RestingTimeActive { get; init; }
    public DateTime? RestingStartTime { get; init; }
    public DateTime? RestingEndTime { get; init; }
}