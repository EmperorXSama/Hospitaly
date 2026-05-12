namespace Hospitaly.Modules.Users.Application.Users.Queries.GetUserInfo;

public record UserResponse(
    Guid UserId,
    string Email,
    string FirstName,
    string LastName,
    string IdentityId,
    string Sex,
    DateOnly DateOfBirth,
    string? BloodType,
    DateTimeOffset CreatedOnUtc);