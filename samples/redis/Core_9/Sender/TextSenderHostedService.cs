using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared.Messages;
using StackExchange.Redis;

class TextSenderHostedService(IMessageSession messageSession, IDatabase db, ILogger<TextSenderHostedService> log) : BackgroundService
{
    const int Megabyte = 1024 * 1024;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        #region set-string
        var key = $"particular:sample:randomtext:{Guid.NewGuid()}";
        var randomText = GetRandomText(1 * Megabyte);

        await db.StringSetAsync(key, randomText);
        #endregion

        #region send-message
        await messageSession.Send(new ProcessText
        {
            RedisKey = key
        }, cancellationToken: stoppingToken);
        #endregion

        log.LogInformation("Sent message with 1MB of random text");
    }

    static string GetRandomText(int length)
    {
        return string.Create(length, length, (chars, state) =>
        {
            var random = new Random();
            for(var i = 0; i < state; i++)
            {
                chars[i] = (char)random.Next('a', 'z');
            }
        });
    }
}
