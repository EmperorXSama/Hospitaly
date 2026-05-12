namespace Hospitaly.Bff.Services;

public sealed record KeycloakRefreshResult(
    bool IsSuccess,
    bool ShouldRevokeSession,
    string? AccessToken,
    string? RefreshToken,
    int? ExpiresInSeconds);

public sealed record KeycloakPasswordGrantResult(
    bool IsSuccess,
    string? AccessToken,
    string? RefreshToken,
    int? ExpiresInSeconds,
    string? ErrorCode = null);
