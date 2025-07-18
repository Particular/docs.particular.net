using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared.Messages;

class TextSenderHostedService(IMessageSession messageSession, ILogger<TextSenderHostedService> log) : BackgroundService
{
    const int Megabyte = 1024 * 1024;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        #region send-message
        await messageSession.Send(new ProcessText
        {
            LargeText = GetRandomText(1 * Megabyte)
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
