using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace Hospitaly.Modules.Users.Infrastructure.Identity;

public class KeycloakAuthDelegatingHandler(
    IOptions<KeycloakOptions> options
    ) : DelegatingHandler
{
    private readonly KeycloakOptions _options = options.Value;
    
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        AuthToken token = await GetAuthorizationToken(cancellationToken);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

        HttpResponseMessage reponseMessage = await base.SendAsync(request, cancellationToken);
        reponseMessage.EnsureSuccessStatusCode();
        
        return reponseMessage;
    }
 //note: this method request access token for each user registration request . for this presentation demo application
 // is fine but in real production we would use cache ( in memory - redis) base on traffic :") ;
    private async Task<AuthToken> GetAuthorizationToken(CancellationToken cancellationToken)
    {
        var authRequestParameters = new KeyValuePair<string, string>[]
        {
            new("client_id", _options.ConfidentialClientId),
            new("client_secret", _options.ConfidentialClientSecret),
            new("scope", "openid"),
            new("grant_type", "client_credentials")
        };

        using var authRequestContent = new FormUrlEncodedContent(authRequestParameters);
        using var authRequets = new HttpRequestMessage(HttpMethod.Post, new Uri(_options.TokenUrl));

        authRequets.Content = authRequestContent;
        
        using HttpResponseMessage authorizationResponse= await base.SendAsync(authRequets, cancellationToken);

        authorizationResponse.EnsureSuccessStatusCode();

        return await authorizationResponse.Content.ReadFromJsonAsync<AuthToken>(cancellationToken);
    }

    internal sealed class AuthToken
    {
        [JsonPropertyName("access_token")] public string AccessToken { get; set; }
    }
}
