using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Sender
{
    public class InputLoopService(IMessageSession messageSession) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key != ConsoleKey.Enter)
                {
                    break;
                }

                var orderSubmitted = new OrderSubmitted
                {
                    OrderId = Guid.NewGuid(),
                    Value = Random.Shared.Next(100)
                };
                await messageSession.Publish(orderSubmitted);
                Console.WriteLine("Published OrderSubmitted message");
            }

        }
    }

}
