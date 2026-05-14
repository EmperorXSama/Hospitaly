using FluentValidation;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.UpdateClinicSpecialty;

public class UpdateClinicSpecialtyCommandValidator : AbstractValidator<UpdateClinicSpecialtyCommand>
{
    public UpdateClinicSpecialtyCommandValidator()
    {
        RuleFor(x => x.ClinicId).NotEmpty();
        RuleFor(x => x.SpecialtyId).NotEmpty();
        RuleFor(x => x.ConsultationFee).GreaterThanOrEqualTo(0).When(x => x.ConsultationFee.HasValue);
    }
}
