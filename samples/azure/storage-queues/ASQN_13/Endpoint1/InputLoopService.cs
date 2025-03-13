using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Endpoint1
{
    public class InputLoopService(IMessageSession messageSession) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Press 'enter' to send a message");
            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key != ConsoleKey.Enter)
                {
                    break;
                }

                var message = new Message1
                {
                    Property = "Hello from Endpoint1"
                };

                await messageSession.Send(message, stoppingToken);

                Console.WriteLine("Message1 sent");
            }
        }
    }
}

