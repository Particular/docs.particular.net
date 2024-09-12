namespace Shared;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

public static class RedisExtensions
{
    #region use-redis
    public static void UseRedis(this IHostApplicationBuilder builder, string configuration)
    {
        var redis = ConnectionMultiplexer.Connect(configuration);

        builder.Services.AddSingleton(redis);
        builder.Services.AddTransient(sp => sp.GetRequiredService<ConnectionMultiplexer>().GetDatabase());
    }
    #endregion
}
