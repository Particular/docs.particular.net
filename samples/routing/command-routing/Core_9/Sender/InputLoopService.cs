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

            Console.WriteLine("Press S to send an order");
            Console.WriteLine("Press C to cancel an order");
            Console.WriteLine("Press ESC to exit");

            var keyPressed = Console.ReadKey(true).Key;

            while (keyPressed != ConsoleKey.Escape)
            {
                switch (keyPressed)
                {
                    case ConsoleKey.S:
                        await PlaceOrder(messageSession, Guid.NewGuid().ToString(), 25m);
                        break;
                    case ConsoleKey.C:
                        await CancelOrder(messageSession, Guid.NewGuid().ToString());
                        break;
                }
                keyPressed = Console.ReadKey(true).Key;
            }


        }
        static async Task PlaceOrder(IMessageSession messageSession, string orderId, decimal value)
        {
            #region send-command-with-configured-route
            var command = new PlaceOrder
            {
                OrderId = orderId,
                Value = value
            };

            await messageSession.Send(command);
            Console.WriteLine("Order placed");
            #endregion
        }

        static async Task CancelOrder(IMessageSession messageSession, string orderId)
        {
            #region send-command-without-configured-route
            var command = new CancelOrder
            {
                OrderId = orderId
            };

            await messageSession.Send("Samples.CommandRouting.Receiver", command);
            Console.WriteLine("Order canceled");
            #endregion
        }
    }

}
