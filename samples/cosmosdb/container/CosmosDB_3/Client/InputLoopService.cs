using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Routing;

namespace Client
{
    public class InputLoopService(IMessageSession messageSession) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();

                var orderId = Guid.NewGuid();
                var startOrder = new StartOrder
                {
                    OrderId = orderId
                };
                if (key.Key == ConsoleKey.S)
                {
                    await messageSession.Send("Samples.CosmosDB.Container.Server", startOrder);
                    Console.WriteLine($"StartOrder Message sent to Server with OrderId {orderId}");
                    continue;
                }
                break;
            }
        }
    }
}
