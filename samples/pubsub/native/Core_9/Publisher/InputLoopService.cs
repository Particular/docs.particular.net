using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Routing;

namespace Publisher
{
    public class InputLoopService(IMessageSession messageSession) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Press '1' to publish the OrderReceived event");

            #region PublishLoop

            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();

                var orderReceivedId = Guid.NewGuid();
                if (key.Key == ConsoleKey.D1)
                {
                    var orderReceived = new OrderReceived
                    {
                        OrderId = orderReceivedId
                    };
                    await messageSession.Publish(orderReceived);
                    Console.WriteLine($"Published OrderReceived Event with Id {orderReceivedId}.");
                }
                else
                {
                    return;
                }
            }

            #endregion

        }
    }

}
