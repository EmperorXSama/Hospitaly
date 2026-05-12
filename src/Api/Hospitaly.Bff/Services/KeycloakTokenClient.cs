using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace Hospitaly.Bff.Services;

public sealed class KeycloakTokenClient(
    HttpClient httpClient,
    IOptionsMonitor<OpenIdConnectSettings> openIdConnectOptions) : IKeycloakTokenClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IOptionsMonitor<OpenIdConnectSettings> _openIdConnectOptions = openIdConnectOptions;

    public async Task<KeycloakRefreshResult> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        OpenIdConnectSettings options = _openIdConnectOptions.CurrentValue;

        using var request = new HttpRequestMessage(HttpMethod.Post, "protocol/openid-connect/token")
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "refresh_token",
                ["client_id"] = options.ClientId,
                ["client_secret"] = options.ClientSecret,
                ["refresh_token"] = refreshToken
            })
        };

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.SendAsync(request, cancellationToken);
        }
        catch (HttpRequestException)
        {
            return new KeycloakRefreshResult(false, false, null, null, null);
        }
        catch (TaskCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            return new KeycloakRefreshResult(false, false, null, null, null);
        }

        if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return new KeycloakRefreshResult(false, true, null, null, null);
        }

        if (!response.IsSuccessStatusCode)
        {
            return new KeycloakRefreshResult(false, false, null, null, null);
        }

        TokenResponse? payload = await response.Content.ReadFromJsonAsync<TokenResponse>(cancellationToken);
        if (payload is null || string.IsNullOrWhiteSpace(payload.AccessToken) || string.IsNullOrWhiteSpace(payload.RefreshToken) || payload.ExpiresIn <= 0)
        {
            return new KeycloakRefreshResult(false, false, null, null, null);
        }

        return new KeycloakRefreshResult(true, false, payload.AccessToken, payload.RefreshToken, payload.ExpiresIn);
    }
    
    private static async Task<string?> TryReadErrorAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        try
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            return string.IsNullOrWhiteSpace(body) ? null : body;
        }
        catch
        {
            return null;
        }
    }

    private sealed class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; init; } = string.Empty;

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; init; } = string.Empty;

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; init; }
    }
}

public sealed class OpenIdConnectSettings
{
    public string Authority { get; init; } = string.Empty;

    public string ClientId { get; init; } = string.Empty;

    public string ClientSecret { get; init; } = string.Empty;
}
