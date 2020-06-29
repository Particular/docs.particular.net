using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;

public class Sender
{
    public static async Task Start(IEndpointInstance endpointInstance)
    {
        var random = new Random();
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        var locations = new[] { "London", "Paris", "Oslo", "Madrid" };

        try
        {

            Console.WriteLine("Press enter to send a message");
            Console.WriteLine("Press any key to exit");

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
                await endpointInstance.SendLocal(orderSubmitted)
                    .ConfigureAwait(false);
            }
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}
