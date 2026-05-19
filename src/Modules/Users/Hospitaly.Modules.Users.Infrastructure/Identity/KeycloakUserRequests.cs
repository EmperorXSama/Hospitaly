using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Hospitaly.Modules.Users.Application.Abstractions.Identity;

namespace Hospitaly.Modules.Users.Infrastructure.Identity;

public class KeycloakUserRequests(HttpClient client) : IUserKeycloakRequests
{
    public async Task<UserTokens?> GetUserTokens(string username, string password)
    {
        var formData = new Dictionary<string, string>
        {
            ["grant_type"] = "password",
            ["client_id"]  = "hospitaly-public-client",
            ["username"]   = username,
            ["password"]   = password,
            ["scope"]   = "email openid",
        };
        var response = await client.PostAsync(
            "protocol/openid-connect/token",
            new FormUrlEncodedContent(formData)
        );
        
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<UserTokens>();
    }
}
