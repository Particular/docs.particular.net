using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace LeftSender
{
    public class InputLoopService(IMessageSession messageSession) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Press '1' to send the PlaceOrder command");
            Console.WriteLine("Press '2' to publish the OrderReceived event");
            Console.WriteLine("Press 'esc' other key to exit");

            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();

                var orderId = Guid.NewGuid();
                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        var placeOrder = new PlaceOrder
                        {
                            OrderId = orderId
                        };
                        await messageSession.Send(placeOrder);
                        Console.WriteLine($"Send PlaceOrder Command with Id {orderId}");
                        break;
                    case ConsoleKey.D2:
                        var orderReceived = new OrderReceived
                        {
                            OrderId = orderId
                        };
                        await messageSession.Publish(orderReceived);
                        Console.WriteLine($"Published OrderReceived Event with Id {orderId}.");
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }


        }
    }

}
