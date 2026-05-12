using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Hospitaly.Bff.Models.DTO;
using Hospitaly.Common.Presentation;
using StackExchange.Redis;

namespace Hospitaly.Bff.Services;

public sealed class UserDataService
{
    private readonly IDatabase _db;
    private readonly HttpClient _client;

    private static readonly TimeSpan Ttl = TimeSpan.FromMinutes(15);

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public UserDataService(
        IConnectionMultiplexer multiplexer,
        HttpClient client)
    {
        _db = multiplexer.GetDatabase();
        _client = client;
    }

    public async Task<ClientUserData> GetUserDataAsync(
        string userId,
        string accessToken,
        CancellationToken cancellationToken = default)
    {
        var key = RedisKeys.UserDataKey(userId);

        RedisValue cached = await _db.StringGetAsync(key);

        if (!cached.IsNullOrEmpty)
        {
            var cachedUserData = JsonSerializer.Deserialize<ClientUserData>(
                (string)cached!,
                JsonOptions);

            if (cachedUserData is not null)
                return cachedUserData;
        }

        var userData = await LoadUserDataFromApiAsync(accessToken, cancellationToken);

        var json = JsonSerializer.Serialize(userData, JsonOptions);

        await _db.StringSetAsync(key, json, Ttl);

        return userData;
    }

    public async Task InvalidateUserDataAsync(string userId)
    {
        await _db.KeyDeleteAsync(RedisKeys.UserDataKey(userId));
    }

    private async Task<ClientUserData> LoadUserDataFromApiAsync(
        string accessToken,
        CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "api/users/me");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var response = await _client.SendAsync(request, cancellationToken);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException("Backend api rejected the access token");
        }

        response.EnsureSuccessStatusCode();

        var envelope = await response.Content.ReadFromJsonAsync<ApiResponse<ClientUserData>>(
            JsonOptions,
            cancellationToken);

        if (envelope is null)
        {
            throw new InvalidOperationException("Backend API returned an empty response body.");
        }

        if (!envelope.IsSuccess)
        {
            throw new InvalidOperationException(
                $"Backend API returned failure: {envelope.Error?.Code} - {envelope.Error?.Message}");
        }

        return envelope.Data ?? throw new InvalidOperationException(
            "Backend API returned success with empty user data.");
    }
}
