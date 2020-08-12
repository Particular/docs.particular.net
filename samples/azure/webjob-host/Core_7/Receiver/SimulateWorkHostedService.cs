using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

class SimulateWorkHostedService : IHostedService
{
    public SimulateWorkHostedService(IMessageSession messageSession)
    {
        this.messageSession = messageSession;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        worker = SimulateWork(cancellationTokenSource.Token);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        cancellationTokenSource.Cancel();
        return worker;
    }

    async Task SimulateWork(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                // sending here to simulate work
                await messageSession.SendLocal(new MyMessage())
                    .ConfigureAwait(false);

                await Task.Delay(1000, cancellationToken).ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException)
        {
            // graceful shutdown
        }
    }

    readonly IMessageSession messageSession;
    readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
    Task worker;
}