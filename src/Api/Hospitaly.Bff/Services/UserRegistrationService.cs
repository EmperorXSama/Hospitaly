using System.Text.Json;
using Hospitaly.Bff.Models.DTO;
using Hospitaly.Common.Presentation;

namespace Hospitaly.Bff.Services;

public sealed class UserRegistrationService
{
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public UserRegistrationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApiResponse<string>> RegisterAsync(
        UserRegistrationRequest request,
        CancellationToken cancellationToken = default)
    {
        using var apiRequest = new HttpRequestMessage(HttpMethod.Post, "api/users/register")
        {
            Content = JsonContent.Create(request, typeof(UserRegistrationRequest), null, JsonOptions)
        };

        var response = await _httpClient.SendAsync(apiRequest, cancellationToken);

        var envelope = await response.Content.ReadFromJsonAsync<ApiResponse<string>>(
            JsonOptions,
            cancellationToken);

        return envelope ?? ApiResponse<string>.Failure(new ApiError(
            "RegistrationFailed",
            "Backend API returned an empty response body."));
    }
}
