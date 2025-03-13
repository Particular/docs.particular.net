using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

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

                if (key.Key != ConsoleKey.Enter)
                {
                    break;
                }

                var orderId = Guid.NewGuid();
                var startOrder = new StartOrder
                {
                    OrderId = orderId
                };
                await messageSession.Send("Samples.RavenDB.Server", startOrder);
                Console.WriteLine($"StartOrder Message sent with OrderId {orderId}");
            }


        }
    }
        
}
