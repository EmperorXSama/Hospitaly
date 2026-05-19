using System.Text.Json.Serialization;

namespace Hospitaly.Modules.Users.Application.Abstractions.Identity;

public interface IUserKeycloakRequests
{
    Task<UserTokens?> GetUserTokens(string username, string password);
}

public class UserTokens
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }
}
