using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared;

namespace Publisher
{
    public class PublisherWorker(IMessageSession messageSession, ILogger<PublisherWorker> logger)
        : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await messageSession.Publish(new MyEvent(), cancellationToken: stoppingToken);

                logger.LogInformation("Published MyOtherEvent");
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                // graceful shutdown
            }
        }
    }
}