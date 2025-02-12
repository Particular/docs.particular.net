using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared;

namespace PublisherV5
{
    public class PublisherWorker : BackgroundService
    {
        private readonly IMessageSession messageSession;
        private readonly ILogger<PublisherWorker> logger;

        public PublisherWorker(IMessageSession messageSession, ILogger<PublisherWorker> logger)
        {
            this.messageSession = messageSession;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await messageSession.Publish(new MyEvent());

                logger.LogInformation("Published MyOtherEvent");
            }
            catch (OperationCanceledException)
            {
                // graceful shutdown
            }
        }
    }
}