
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Routing;
public class InputLoopService(IMessageSession messageSession) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        Console.WriteLine("Press 'Enter' to send a new message. Press any other key to finish.");
        while (true)
        {
            var key = Console.ReadKey();

            if (key.Key != ConsoleKey.Enter)
            {
                break;
            }

            var guid = Guid.NewGuid();

            var simpleMessage = new SimpleMessage
            {
                Id = guid
            };
            await messageSession.Send("NServiceBusEndpoint", simpleMessage);
            Console.WriteLine($"Sent a new message with Id = {guid}.");

            Console.WriteLine("Press 'Enter' to send a new message");
        }
    }
}
