using System.Net.Http.Json;

namespace Hospitaly.Modules.Users.Infrastructure.Identity;

public sealed class KeyCloakClient (HttpClient httpClient)
{
    internal async Task<string> RegisterUserAsync(UserRepresentation user, CancellationToken cancellationToken)
    {
        HttpResponseMessage responseMessage = await httpClient.PostAsJsonAsync(
            "users",
            user,
            cancellationToken

        );

        responseMessage.EnsureSuccessStatusCode();

        return ExtractIdentityId(responseMessage);
    }

    private static string ExtractIdentityId(HttpResponseMessage response)
    {
        const string userFragment = "users/";
        var locationHeader = response.Headers.Location?.PathAndQuery;
        if (locationHeader is null)
        {
            throw new InvalidOperationException("location header is empty");
        }

        var getUserFragmentIndex = locationHeader.IndexOf(userFragment, StringComparison.InvariantCultureIgnoreCase);
        var identityId = locationHeader.Substring(getUserFragmentIndex + userFragment.Length);
        return identityId;
    }
}