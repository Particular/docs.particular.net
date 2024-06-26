using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

#region SendMessageJob
class SendMessageJob(IMessageSession messageSession) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(3));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await messageSession.Send("Receiver", new MyMessage(), stoppingToken);
        }
    }
}
#endregion
