using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Sender;

class InputLoopService(IMessageSession messageSession) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var random = new Random();

        while (true)
        {
            if (!Console.KeyAvailable)
            {
                await Task.Delay(100, stoppingToken);
                continue;
            }
            var key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key != ConsoleKey.Enter)
            {
                break;
            }

            var orderSubmitted = new OrderSubmitted(
                OrderId: Guid.NewGuid(),
                Value: random.Next(100)
            );

            await messageSession.Publish(orderSubmitted, cancellationToken: stoppingToken);
        }
    }
}