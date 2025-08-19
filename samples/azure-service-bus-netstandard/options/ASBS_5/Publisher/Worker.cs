using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Shared;

class Worker(IMessageSession messageSession) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            var number = 0;

            while (!cancellationToken.IsCancellationRequested)
            {
                await messageSession.Publish(new EventOne
                {
                    Content = $"EventOne {number++}",
                    PublishedOnUtc = DateTime.UtcNow
                }, cancellationToken);

                await Task.Delay(1000, cancellationToken);

                await messageSession.Publish(new EventTwo
                {
                    Content = $"EventTwo {number}",
                    PublishedOnUtc = DateTime.UtcNow
                }, cancellationToken);
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            // graceful shutdown
        }
    }
}