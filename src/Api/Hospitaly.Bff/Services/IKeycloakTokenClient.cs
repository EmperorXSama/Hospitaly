namespace Hospitaly.Bff.Services;

public interface IKeycloakTokenClient
{
    Task<KeycloakRefreshResult> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);

   
}
