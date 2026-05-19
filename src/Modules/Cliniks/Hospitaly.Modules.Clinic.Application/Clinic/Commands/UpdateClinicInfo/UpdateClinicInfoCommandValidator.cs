using FluentValidation;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.UpdateClinicInfo;

public class UpdateClinicInfoCommandValidator : AbstractValidator<UpdateClinicInfoCommand>
{
    public UpdateClinicInfoCommandValidator()
    {
        RuleFor(x => x.ClinicId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.TradingName).MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(1000);
        RuleFor(x => x.LogoUrl).MaximumLength(500);
    }
}
