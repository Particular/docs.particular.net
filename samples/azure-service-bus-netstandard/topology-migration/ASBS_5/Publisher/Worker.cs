using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared;

namespace Publisher
{
    public class Worker(IMessageSession messageSession, ILogger<Worker> logger)
        : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                var number = 0;
                while (!cancellationToken.IsCancellationRequested)
                {
                    var myEvent = new MyEvent
                    {
                        Content = $"MyEvent {number++}",
                        PublishedOnUtc = DateTime.UtcNow
                    };
                    await messageSession.Publish(myEvent, cancellationToken);

                    logger.LogInformation("Published MyEvent {{ Content = {Content}, PublishedOnUtc = {PublishedOnUtc} }}", myEvent.Content, myEvent.PublishedOnUtc);

                    await Task.Delay(1000, cancellationToken);
                }
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                // graceful shutdown
            }
        }
    }
}