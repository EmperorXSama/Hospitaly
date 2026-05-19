using FluentValidation;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.UpdateOperatingLicenseStatus;

public class UpdateOperatingLicenseStatusCommandValidator : AbstractValidator<UpdateOperatingLicenseStatusCommand>
{
    public UpdateOperatingLicenseStatusCommandValidator()
    {
        RuleFor(x => x.ClinicId).NotEmpty();
    }
}
