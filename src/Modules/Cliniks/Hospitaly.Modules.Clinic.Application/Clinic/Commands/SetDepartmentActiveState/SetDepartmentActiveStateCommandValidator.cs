using FluentValidation;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.SetDepartmentActiveState;

public class SetDepartmentActiveStateCommandValidator : AbstractValidator<SetDepartmentActiveStateCommand>
{
    public SetDepartmentActiveStateCommandValidator()
    {
        RuleFor(x => x.ClinicId).NotEmpty();
        RuleFor(x => x.DepartmentId).NotEmpty();
    }
}
