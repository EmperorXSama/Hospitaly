using System.Text.RegularExpressions;
using FluentValidation;
using Hospitaly.Common.Domain.Common;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.CreateClinic;

public class CreateClinicCommandValidator : AbstractValidator<CreateClinicCommand>
{
    public CreateClinicCommandValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty();
        RuleFor(x => x.Email).NotNull()
            .NotEmpty()
            .EmailAddress();
        RuleFor(x => x.Phone)
            .NotEmpty()
            .Must(x => Regex.IsMatch(x, @"^(\+212|0)[5-7]\d{8}$"));
        RuleFor(x => x.Description)
            .MaximumLength(1000);

        RuleFor(x => x.Street)
            .NotEmpty();
        RuleFor(x => x.Country)
            .NotEmpty();
        
        RuleFor(x => x.Region)
            .NotEmpty()
            .Must(AddressValidator.IsValidRegion)
            .When(x => IsMorocco(x.Country))
            .WithMessage("Region must be a valid Moroccan region.");
        RuleFor(x => x.City)
            .NotEmpty()
            .Must((command , city) => AddressValidator.IsValidCity(command.Region, city))
            .WithMessage("City must be a valid Moroccan region.");
    }
    private static bool IsMorocco(string? country)
    {
        return string.Equals(country?.Trim(), "Morocco", StringComparison.OrdinalIgnoreCase)
               || string.Equals(country?.Trim(), "MA", StringComparison.OrdinalIgnoreCase);
    }
    
}