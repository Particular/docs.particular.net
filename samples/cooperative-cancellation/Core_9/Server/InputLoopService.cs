using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Routing;

namespace Server
{
    public class InputLoopService(IMessageSession messageSession, ILogger<InputLoopService> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var message = new LongRunningMessage { DataId = Guid.NewGuid() };


            #region StoppingEndpointWithCancellationToken
            var tokenSource = new CancellationTokenSource();
            tokenSource.CancelAfter(TimeSpan.FromSeconds(1));
            #endregion

            await messageSession.SendLocal(new LongRunningMessage { DataId = Guid.NewGuid() }, tokenSource.Token);
            Console.ReadKey();

            logger.LogInformation("Giving the endpoint 1 second to gracefully stop before sending a cancel signal to the cancellation token");
        }
    }
}