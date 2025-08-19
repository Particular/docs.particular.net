using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Messages.Events;
using Microsoft.Extensions.Hosting;

namespace MTEndpoint
{
    public class Worker : BackgroundService
    {
        readonly IBus bus;


        public Worker(IBus bus)
        {
            this.bus = bus;
        }

        #region MassTransitPublish

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await bus.Publish(new MassTransitEvent { Text = $"The time is {DateTimeOffset.Now}" });
                await Task.Delay(1000, stoppingToken);
            }
        }

        #endregion
    }
}
