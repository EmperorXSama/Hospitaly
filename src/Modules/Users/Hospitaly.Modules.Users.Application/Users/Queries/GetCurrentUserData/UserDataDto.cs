namespace Hospitaly.Modules.Users.Application.Users.Queries.GetCurrentUserData;

public sealed record UserDataDto(
    string UserId,
    string UserName,
    string Email,
    IReadOnlyCollection<string> Roles,
    IReadOnlyCollection<string> Permissions,
    bool RequiresOnboarding);
