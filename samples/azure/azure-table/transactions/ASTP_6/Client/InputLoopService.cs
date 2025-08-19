
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;
public class InputLoopService(IMessageSession messageSession) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Press 'S' to send a StartOrder message to the server endpoint");

        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            var orderId = Guid.NewGuid();
            var startOrder = new StartOrder
            {
                OrderId = orderId
            };
            if (key.Key == ConsoleKey.S)
            {
                await messageSession.Send("Samples.AzureTable.Transactions.Server", startOrder);
                Console.WriteLine($"StartOrder Message sent to Server with OrderId {orderId}");
                continue;
            }
            break;
        }


    }
}
