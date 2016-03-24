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
        Random random = new Random();
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Samples.MultiTenant.Sender");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.SendFailedMessagesTo("error");

        endpointConfiguration.UsePersistence<NHibernatePersistence>();
        endpointConfiguration.EnableOutbox();

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);

        try
        {
            Console.WriteLine("Press A or B to publish a message (A and B are tenant IDs)");
            List<char> acceptableInput = new List<char> { 'A', 'B' };

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine();
                char uppercaseKey = char.ToUpperInvariant(key.KeyChar);

                if (acceptableInput.Contains(uppercaseKey))
                {
                    string orderId = new string(Enumerable.Range(0, 4).Select(x => letters[random.Next(letters.Length)]).ToArray());
                    OrderSubmitted message = new OrderSubmitted
                    {
                        OrderId = orderId,
                        Value = random.Next(100)
                    };

                    PublishOptions options = new PublishOptions();
                    options.SetHeader("TenantId", uppercaseKey.ToString());

                    await endpoint.Publish(message, options);
                }
                else
                {
                    Console.WriteLine($"[{uppercaseKey}] is not a valid tenant identifier.");
                }
            }
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}