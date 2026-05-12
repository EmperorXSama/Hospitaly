using FluentValidation;

namespace Hospitaly.Modules.Users.Application.Users.Commands.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(user => user.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(user => user.Password)
            .NotEmpty()
            .MinimumLength(6);

        RuleFor(x => x.Sex)
            .NotEmpty();

        RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .LessThan(DateOnly.FromDateTime(DateTime.UtcNow));

        RuleFor(x => x.BloodType)
            .Must(bt => string.IsNullOrEmpty(bt) || bt.Length <= 10)
            .When(x => x.BloodType is not null);
    }
}