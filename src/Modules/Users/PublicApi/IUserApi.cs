namespace PublicApi;

public interface IUserApi
{
    Task<UserResponseDto?> GetUserDataByIdentityIdAsync(string identityId, CancellationToken cancellationToken = default);
    Task<List<UserResponseDto>> SearchUsersByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task AddClinicOwnerRole(string identityId, CancellationToken cancellationToken = default);
    Task AddDoctorRole(string identityId, CancellationToken cancellationToken = default);
}

public sealed class UserResponseDto
{
    public Guid UserId { get; init; }
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string IdentityId { get; init; } = string.Empty;
    public string Sex { get; init; } = string.Empty;
    public DateOnly DateOfBirth { get; init; }
    public DateTimeOffset CreatedOnUtc { get; init; }
}