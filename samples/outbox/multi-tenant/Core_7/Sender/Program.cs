using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.MultiTenant.Sender";
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        var random = new Random();
        var endpointConfiguration = new EndpointConfiguration("Samples.MultiTenant.Sender");
        endpointConfiguration.SendFailedMessagesTo("error");

        endpointConfiguration.UsePersistence<NHibernatePersistence>();
        endpointConfiguration.UseTransport<MsmqTransport>();
        endpointConfiguration.EnableOutbox();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press A or B to publish a message (A and B are tenant IDs)");
        Console.WriteLine("Press Escape to exit");
        var acceptableInput = new List<char> { 'A', 'B' };

        while (true)
        {

            var key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key == ConsoleKey.Escape)
            {
                break;
            }
            var uppercaseKey = char.ToUpperInvariant(key.KeyChar);

            if (acceptableInput.Contains(uppercaseKey))
            {
                var orderId = new string(Enumerable.Range(0, 4).Select(x => letters[random.Next(letters.Length)]).ToArray());
                var message = new OrderSubmitted
                {
                    OrderId = orderId,
                    Value = random.Next(100)
                };

                var options = new PublishOptions();
                options.SetHeader("TenantId", uppercaseKey.ToString());

                await endpointInstance.Publish(message, options)
                    .ConfigureAwait(false);
            }
            else
            {
                Console.WriteLine($"[{uppercaseKey}] is not a valid tenant identifier.");
            }
        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}