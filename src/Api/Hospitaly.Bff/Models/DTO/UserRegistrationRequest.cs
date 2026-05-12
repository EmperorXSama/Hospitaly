namespace Hospitaly.Bff.Models.DTO;

public sealed record UserRegistrationRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string Sex,
    DateOnly DateOfBirth,
    string? BloodType);
