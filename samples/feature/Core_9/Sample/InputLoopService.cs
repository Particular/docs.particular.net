using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Routing;

namespace Sample
{
    public class InputLoopService(IMessageSession messageSession) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            Console.WriteLine("Press 'H' to send a HandlerMessage");
            Console.WriteLine("Press 'S' to send a StartSagaMessage");
            Console.WriteLine("Press any other key to exit");

            while (true)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.H)
                {
                    await SendHandlerMessage(messageSession);
                    continue;
                }
                if (key.Key == ConsoleKey.S)
                {
                    await SendSagaMessage(messageSession);
                    continue;
                }
                return;
            }

        }

        static Task SendHandlerMessage(IMessageSession endpointInstance)
        {
            Console.WriteLine();
            Console.WriteLine("HandlerMessage sent");
            var message = new HandlerMessage();
            return endpointInstance.SendLocal(message);
        }

        static Task SendSagaMessage(IMessageSession endpointInstance)
        {
            Console.WriteLine();
            Console.WriteLine("StartSagaMessage sent");
            var message = new StartSagaMessage
            {
                SentTime = DateTimeOffset.UtcNow,
                TheId = Guid.NewGuid()
            };
            return endpointInstance.SendLocal(message);
        }
    }
}
