using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

class SimulateWorkHostedService : IHostedService
{
    public SimulateWorkHostedService(IServiceProvider provider)
    {
        this.provider = provider;
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
            // we can only retrieve it here because the job host is started before NServiceBus is started
            var session = provider.GetService<IMessageSession>();

            while (!cancellationToken.IsCancellationRequested)
            {
                // sending here to simulate work
                await session.SendLocal(new MyMessage())
                    .ConfigureAwait(false);

                await Task.Delay(1000, cancellationToken).ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException)
        {
            // graceful shutdown
        }
    }

    readonly IServiceProvider provider;
    readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
    Task worker;
}