using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Hospitaly.Bff.Controllers;

[ApiController]
[Route("test")]
public class TestController: ControllerBase
{
    
    [HttpGet]
    public async Task<ActionResult> GetTest()
    {
        return Ok(new { Messgage = "Data from api" });
    }
    
[HttpGet("redis-debug")]
[AllowAnonymous]
public async Task<IActionResult> DebugRedis([FromServices] IConnectionMultiplexer redis)
{
    var db = redis.GetDatabase();
    var server = redis.GetServer(redis.GetEndPoints().First());

    // --- Connection Info ---
    var endpoint = redis.GetEndPoints().First();
    var connectionInfo = new
    {
        endpoint = endpoint.ToString(),
        isConnected = redis.IsConnected,
        databaseIndex = db.Database,
        serverVersion = server.Version.ToString(),
        serverMode = server.ServerType.ToString(),
    };

    // --- Write a fresh test key ---
    await db.StringSetAsync("debug:probe", "alive", TimeSpan.FromMinutes(2));

    // --- Scan ALL keys in current DB ---
    var keys = server.Keys(database: db.Database, pattern: "*").ToList();

    var keyValues = new List<object>();
    foreach (var key in keys)
    {
        var type = await db.KeyTypeAsync(key);
        var ttl = await db.KeyTimeToLiveAsync(key);

        object? val = type switch
        {
            RedisType.String => (string?)await db.StringGetAsync(key),
            RedisType.Hash   => await db.HashGetAllAsync(key)
                                        .ContinueWith(t => (object)t.Result
                                            .ToDictionary(e => e.Name.ToString(), e => e.Value.ToString())),
            RedisType.List   => await db.ListRangeAsync(key)
                                        .ContinueWith(t => (object)t.Result.Select(v => v.ToString())),
            RedisType.Set    => await db.SetMembersAsync(key)
                                        .ContinueWith(t => (object)t.Result.Select(v => v.ToString())),
            RedisType.SortedSet => await db.SortedSetRangeByRankWithScoresAsync(key)
                                           .ContinueWith(t => (object)t.Result
                                               .Select(e => new { member = e.Element.ToString(), score = e.Score })),
            _ => $"[unsupported type: {type}]"
        };

        keyValues.Add(new
        {
            key = key.ToString(),
            type = type.ToString(),
            ttlSeconds = ttl?.TotalSeconds,
            value = val
        });
    }

    return Ok(new
    {
        connection = connectionInfo,
        totalKeys = keyValues.Count,
        keys = keyValues
    });
}
}