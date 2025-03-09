using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Sample
{
    public class InputLoopService(IMessageSession messageSession) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                Console.WriteLine();
                if (Console.ReadKey().Key != ConsoleKey.Enter)
                {
                    break;
                }
                var orderId = Guid.NewGuid();
                var startOrder = new StartOrder
                {
                    OrderId = orderId
                };
                await messageSession.SendLocal(startOrder);
                Console.WriteLine($"Sent StartOrder with OrderId {orderId}.");
            }

        }
    }

}
