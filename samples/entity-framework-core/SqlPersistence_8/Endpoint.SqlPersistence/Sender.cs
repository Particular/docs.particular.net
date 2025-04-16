using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Routing;

public class Sender(IMessageSession messageSession) : BackgroundService
{


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        var random = new Random();
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        var locations = new[] { "London", "Paris", "Oslo", "Madrid" };

        try
        {

            Console.WriteLine("Press enter to send a message");

            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }
                var orderId = new string(Enumerable.Range(0, 4).Select(x => letters[random.Next(letters.Length)]).ToArray());
                var shipTo = locations[random.Next(locations.Length)];
                var orderSubmitted = new OrderSubmitted
                {
                    OrderId = orderId,
                    Value = random.Next(100),
                    ShipTo = shipTo
                };
                await messageSession.SendLocal(orderSubmitted);
            }
        }
        finally
        {

        }
    }
}
