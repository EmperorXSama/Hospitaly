using FluentValidation;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.ReplaceOperatingLicense;

public class ReplaceOperatingLicenseCommandValidator : AbstractValidator<ReplaceOperatingLicenseCommand>
{
    public ReplaceOperatingLicenseCommandValidator()
    {
        RuleFor(x => x.ClinicId).NotEmpty();
        RuleFor(x => x.LicenseNumber).NotEmpty().MaximumLength(100);
        RuleFor(x => x.IssuingAuthority).NotEmpty().MaximumLength(200);
    }
}
