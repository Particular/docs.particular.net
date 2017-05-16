using System;
using System.Threading.Tasks;
using NServiceBus;

static class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.TransientClients.Publisher";
        var endpointConfiguration = new EndpointConfiguration("Samples.TransientClients.Publisher");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseTransport<MsmqTransport>();

        endpointConfiguration.SendFailedMessagesTo("error");

        endpointConfiguration.Conventions().DefiningEventsAs(t => t == typeof(StockTick));

        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        await Start(endpointInstance)
            .ConfigureAwait(false);
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }

    static async Task Start(IEndpointInstance endpointInstance)
    {
        #region PublishLoop

        var rand = new Random();

        Console.WriteLine("Press any key to start publishing");
        Console.ReadKey(true);

        do
        {
            while (!Console.KeyAvailable)
            {
                var stockSymbol = Symbols[rand.Next(0, Symbols.Length - 1)];

                var orderReceived = new StockTick
                {
                    Symbol = stockSymbol,
                    Timestamp = DateTime.UtcNow
                };
                await endpointInstance.Publish(orderReceived)
                    .ConfigureAwait(false);

                await Task.Delay(500);

                Console.WriteLine($"Published StockUpdated Event with Symbol {stockSymbol}. Press escape to stop publishing events.");
            }
        } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

        #endregion
    }

    private static string[] Symbols = new[]
    {
        "MSFT",
        "AAPL",
        "GOOGL",
        "ORCL",
        "INTC",
        "HPQ",
        "CSCO"
    };
}