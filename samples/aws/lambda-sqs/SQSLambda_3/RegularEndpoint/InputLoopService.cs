
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;
public class InputLoopService(IMessageSession messageSession) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        Console.WriteLine("Press [ENTER] to send a message to the serverless endpoint queue.");
        Console.WriteLine("Press [Esc] to exit.");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();
            switch (key.Key)
            {
                case ConsoleKey.Enter:
                    await messageSession.Send("ServerlessEndpoint", new TriggerMessage());
                    Console.WriteLine("Message sent to the serverless endpoint queue.");
                    break;
                case ConsoleKey.Escape:
                    return;
            }
        }
    }
}
