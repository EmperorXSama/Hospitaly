namespace Hospitaly.Modules.Users.Presentation;

public sealed record RegisterUserRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string Sex,
    DateOnly DateOfBirth,
    string? BloodType);
