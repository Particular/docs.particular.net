namespace Shared;

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus.ClaimCheck;
using StackExchange.Redis;

#region claim-check
public class RedisClaimCheck(string configuration) : IClaimCheck
{
    private IConnectionMultiplexer redis;

    public async Task<Stream> Get(string key, CancellationToken cancellationToken = default)
    {
        var db = redis.GetDatabase();
        byte[] redisValue = await db.StringGetAsync(key);
        return new MemoryStream(redisValue);
    }

    public async Task<string> Put(Stream stream, TimeSpan timeToBeReceived, CancellationToken cancellationToken = default)
    {
        var db = redis.GetDatabase();
        var key = $"particular:sample:claimcheck:{Guid.NewGuid()}";
        var value = ((MemoryStream)stream).ToArray();

        await db.StringSetAsync(key, value, timeToBeReceived == TimeSpan.MaxValue ? null : timeToBeReceived, false);
        return key;
    }

    public async Task Start(CancellationToken cancellationToken = default)
    {
        redis = await ConnectionMultiplexer.ConnectAsync(configuration);
    }
}
#endregion