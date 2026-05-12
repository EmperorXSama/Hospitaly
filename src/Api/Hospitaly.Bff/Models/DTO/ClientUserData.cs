namespace Hospitaly.Bff.Models.DTO;

public sealed class ClientUserData
{
    public string UserId { get; init; } = string.Empty;

    public string UserName { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public IReadOnlyCollection<string> Roles { get; init; } = [];

    public IReadOnlyCollection<string> Permissions { get; init; } = [];

    public bool RequiresOnboarding { get; init; }
}