using System.Text.Json;
using Hospitaly.Bff.Models;
using Hospitaly.Bff.Models.DTO;
using StackExchange.Redis;

namespace Hospitaly.Bff.Services;

public class SessionService (IConnectionMultiplexer redis, IKeycloakTokenClient keycloakTokenClient)
{
    private readonly IDatabase _db = redis.GetDatabase();
    private readonly IKeycloakTokenClient _keycloakTokenClient = keycloakTokenClient;
    private static readonly TimeSpan SessionTtl = TimeSpan.FromDays(7);



    public async Task CreateSessionAsync(
        string sessionId,
        string userId,
        string accessToken,
        string refreshToken,
        DateTime tokenExpiresAt,
        string ipAddress,
        string userAgent)
    {
        var parser = UAParser.Parser.GetDefault();
        var client = parser.Parse(userAgent);

        var session = new UserSession
        {
            SessionId = sessionId,
            UserId = userId,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            TokenExpiresAt = tokenExpiresAt,
            IpAddress = ipAddress,
            Browser = $"{client.UA.Family} {client.UA.Major}",
            Os = $"{client.OS.Family} {client.OS.Major}",
            Device = client.Device.Family,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.Add(SessionTtl)
        };

        var json = JsonSerializer.Serialize(session);
        await _db.StringSetAsync($"session:{sessionId}", json, SessionTtl);
        await _db.SetAddAsync($"user_sessions:{userId}", sessionId);
    }

    public async Task<List<SessionDto>> GetUserSessionsAsync(string userId, string currentSessionId)
    {
        var sessionIds = await _db.SetMembersAsync($"user_sessions:{userId}");
        var sessions = new List<SessionDto>();
        var staleIds = new List<RedisValue>();

        foreach (var id in sessionIds)
        {
            var json = await _db.StringGetAsync($"session:{id}");

            if (json.IsNullOrEmpty)
            {
                // session expired or was deleted — remove from Set
                staleIds.Add(id);
                continue;
            }

            var session = JsonSerializer.Deserialize<UserSession>((string)json!);
            if (session is null) continue;

            session.IsCurrent = session.SessionId == currentSessionId;
            sessions.Add(new SessionDto
            {
                SessionId = session.SessionId,
                Browser = session.Browser,
                Os = session.Os,
                Device = session.Device,
                IpAddress = session.IpAddress,
                CreatedAt = session.CreatedAt,
                ExpiresAt = session.ExpiresAt,
                IsCurrent = session.SessionId == currentSessionId
            });
        }

        // clean up stale references
        foreach (var staleId in staleIds)
            await _db.SetRemoveAsync($"user_sessions:{userId}", staleId);

        return sessions.OrderByDescending(s => s.CreatedAt).ToList();
    }
    public async Task<bool> RevokeSessionAsync(string sessionId, string userId)
    {
        // make sure session belongs to this user before deleting
        var isMember = await _db.SetContainsAsync($"user_sessions:{userId}", sessionId);
        if (!isMember) return false;

        await _db.KeyDeleteAsync($"session:{sessionId}");
        await _db.SetRemoveAsync($"user_sessions:{userId}", sessionId);
        return true;
    }

    public async Task RevokeAllSessionsAsync(string userId)
    {
        var sessionIds = await _db.SetMembersAsync($"user_sessions:{userId}");

        foreach (var id in sessionIds)
            await _db.KeyDeleteAsync($"session:{id}");

        await _db.KeyDeleteAsync($"user_sessions:{userId}");
    }
    
    public async Task<UserSession?> GetSessionAsync(string sessionId)
    {
        RedisValue json = await _db.StringGetAsync($"session:{sessionId}");
        if (json.IsNullOrEmpty) return null;
        return JsonSerializer.Deserialize<UserSession>((string)json!);
    }

    public async Task UpdateSessionTokensAsync(string sessionId, string accessToken, string refreshToken, DateTime expiresAt)
    {
        var session = await GetSessionAsync(sessionId);
        if (session is null) return;

        session.AccessToken = accessToken;
        session.RefreshToken = refreshToken;
        session.TokenExpiresAt = expiresAt;

        var json = JsonSerializer.Serialize(session);
        var remainingTtl = session.ExpiresAt - DateTime.UtcNow;
        await _db.StringSetAsync($"session:{sessionId}", json, remainingTtl);
    }

    public async Task<UserSession?> RefreshSessionTokenAsync(UserSession session, CancellationToken cancellationToken = default)
    {
        KeycloakRefreshResult result = await _keycloakTokenClient.RefreshTokenAsync(session.RefreshToken, cancellationToken);

        if (!result.IsSuccess)
        {
            if (result.ShouldRevokeSession)
            {
                await RevokeSessionAsync(session.SessionId, session.UserId);
            }

            return null;
        }

        var newAccessToken = result.AccessToken!;
        var newRefreshToken = result.RefreshToken!;
        var expiresIn = result.ExpiresInSeconds!.Value;
        var newExpiry = DateTime.UtcNow.AddSeconds(expiresIn);

        await UpdateSessionTokensAsync(session.SessionId, newAccessToken, newRefreshToken, newExpiry);

        session.AccessToken = newAccessToken;
        session.RefreshToken = newRefreshToken;
        session.TokenExpiresAt = newExpiry;

        return session;
    }
}
