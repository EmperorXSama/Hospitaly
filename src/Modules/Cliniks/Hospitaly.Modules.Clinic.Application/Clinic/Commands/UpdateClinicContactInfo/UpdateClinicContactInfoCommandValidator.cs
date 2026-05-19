using FluentValidation;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.UpdateClinicContactInfo;

public class UpdateClinicContactInfoCommandValidator : AbstractValidator<UpdateClinicContactInfoCommand>
{
    public UpdateClinicContactInfoCommandValidator()
    {
        RuleFor(x => x.ClinicId).NotEmpty();
    }
}
