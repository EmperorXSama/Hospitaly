using FluentValidation;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.SetClinicOperatingHours;

public class SetClinicOperatingHoursCommandValidator :AbstractValidator<SetClinicOperatingHoursCommand>
{
      public SetClinicOperatingHoursCommandValidator()
      {
        RuleFor(o => o.OperatingHours)
         .NotEmpty()
         .WithMessage("Operating Hours cannot be empty")
         .Must(hours =>
         {
             var seen = new HashSet<DayOfWeek>();
             return hours.All(h => seen.Add(h.Day));
         })
         .WithMessage("Each day of the week can only appear once.");
        RuleForEach(o => o.OperatingHours)
         .SetValidator(new OperatingHoursValidator());
     }


}
public class OperatingHoursValidator : AbstractValidator<OperatingHoursDto>
{
    public OperatingHoursValidator()
    {
        RuleFor(o => o.StartTime)
            .Must( ValidTimeRange)
            .When(o => o.StartTime.HasValue)
            .WithMessage("Start Time is required for operating hours.");
        RuleFor(o => o.RestingEndsAt)
            .Must( ValidTimeRange)
            .When(o => o.RestingEndsAt.HasValue)
            .WithMessage("RestingEndsAt Time is required for operating hours.");
        RuleFor(o => o.EndTime)
            .Must( ValidTimeRange)
            .When(o => o.EndTime.HasValue)
            .WithMessage("EndTime Time is required for operating hours.");
        RuleFor(o => o.RestingStartsAt)
            .Must( ValidTimeRange)
            .When(o => o.RestingStartsAt.HasValue)
            .WithMessage("RestingStartsAt Time is required for operating hours.");
        
        RuleFor(o => o)
            .Must(o => o.StartTime< o.EndTime)
            .When(o => o.StartTime.HasValue && o.EndTime.HasValue)
            .WithMessage("Resting time must be within operating hours.");

        
        RuleFor(o => o)
            .Must(o => o.RestingStartsAt >= o.StartTime && o.RestingStartsAt <= o.EndTime)
            .When(o => o.RestingEndsAt.HasValue && o.RestingEndsAt.HasValue)
            .WithMessage("Start Time is required for operating hours.");
        RuleFor(o => o.RestingStartsAt)
            .NotNull()
            .When(o => o.RestingEndsAt.HasValue)
            .WithMessage("Resting start time is required when resting end time is provided.");

        RuleFor(o => o.RestingEndsAt)
            .NotNull()
            .When(o => o.RestingStartsAt.HasValue)
            .WithMessage("Resting end time is required when resting start time is provided.");
        RuleFor(o => o.StartTime)
            .NotNull()
            .When(o => !o.IsClosed)
            .WithMessage("Start time is required for open days.");
    }

    private bool ValidTimeRange(TimeSpan? time)
    {
        return time.HasValue 
               && time.Value >= TimeSpan.Zero 
               && time.Value < TimeSpan.FromHours(24);
    }
}


