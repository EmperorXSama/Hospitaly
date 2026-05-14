using FluentValidation;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.AddClinicSpecialty;

public class AddClinicSpecialtyCommandValidator : AbstractValidator<AddClinicSpecialtyCommand>
{
    public AddClinicSpecialtyCommandValidator()
    {
        RuleFor(x => x.ClinicId).NotEmpty();
        RuleFor(x => x.SpecialtyId).NotEmpty();
        RuleFor(x => x.ConsultationFee).GreaterThanOrEqualTo(0).When(x => x.ConsultationFee.HasValue);
    }
}
