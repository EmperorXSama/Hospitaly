namespace Hospitaly.Modules.Users.Application.Users.Queries.GetUserInfo;

public sealed class UserResponse
{
    public Guid UserId { get; init; }
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string IdentityId { get; init; } = string.Empty;
    public string Sex { get; init; } = string.Empty;
    public DateOnly DateOfBirth { get; init; }
    public string? BloodType { get; init; }
    public DateTimeOffset CreatedOnUtc { get; init; }
    public bool RequiresOnboarding { get; init; }
}