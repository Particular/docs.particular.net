using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class Program
{

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.CustomRouting.Client";
        LogManager.Use<DefaultFactory>()
            .Level(LogLevel.Info);

        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        var random = new Random();

        var endpointConfiguration = new EndpointConfiguration("Samples.CustomRouting.Client");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        #region EnableAutomaticRouting
        endpointConfiguration.EnableAutomaticRouting(@"Data Source=.\SQLEXPRESS;Initial Catalog=AutomaticRouting;Integrated Security=True");
        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        try
        {
            Console.WriteLine("Press enter to send a message");
            Console.WriteLine("Press any key to exit");

            #region ClientLoop

            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }
                var orderId = new string(Enumerable.Range(0, 4).Select(x => letters[random.Next(letters.Length)]).ToArray());
                Console.WriteLine($"Placing order {orderId}");
                var message = new PlaceOrder
                {
                    OrderId = orderId,
                    Value = random.Next(100)
                };
                await endpointInstance.Send(message).ConfigureAwait(false);
            }

            #endregion
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}