using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Messages;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Routing;

namespace Sender
{
    public class InputLoopService(IMessageSession messageSession) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                if (Console.ReadKey().Key != ConsoleKey.Enter)
                {
                    break;
                }
                await PlaceOrder(messageSession);
            }

        }
        static async Task PlaceOrder(IMessageSession messageSession)
        {
            #region SendMessage

            var order = new ClientOrder
            {
                OrderId = Guid.NewGuid()
            };
            await messageSession.Send(order);

            #endregion

            Console.WriteLine($"ClientOrder message sent with ID {order.OrderId}");
        }
    }
}
