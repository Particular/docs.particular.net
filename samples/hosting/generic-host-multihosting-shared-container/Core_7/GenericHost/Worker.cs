using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

#region generic-host-worker
class Worker : BackgroundService
{
    private readonly ISessionProvider provider;

    public Worker(ISessionProvider provider)
    {
        this.provider = provider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var number = 0;
            var toggle = false;
            while (!stoppingToken.IsCancellationRequested)
            {
                var session = provider.GetSession($"Samples.Hosting.GenericMultiHost.Host{(toggle ? "1" : "2")}");

                await session.SendLocal(new MyMessage {Number = number++})
                    .ConfigureAwait(false);

                await Task.Delay(1000, stoppingToken).ConfigureAwait(false);

                toggle = !toggle;
            }
        }
        catch (OperationCanceledException)
        {
            // graceful shutdown
        }
    }
}
#endregion