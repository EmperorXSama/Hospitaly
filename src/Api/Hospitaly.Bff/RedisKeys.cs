namespace Hospitaly.Bff;

public class RedisKeys
{
    public static string SessionKey(string sessionId)
        => $"session:{sessionId}";

    public static string UserSessionsKey(string userId)
        => $"user_sessions:{userId}";

    public static string UserDataKey(string userId)
        => $"client_user_data:{userId}";
}