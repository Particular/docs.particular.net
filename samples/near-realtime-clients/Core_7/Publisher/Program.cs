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
        Console.Title = "Samples.NearRealTimeClients.Publisher";
        var endpointConfiguration = new EndpointConfiguration("Samples.NearRealTimeClients.Publisher");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseTransport<MsmqTransport>();

        endpointConfiguration.SendFailedMessagesTo("error");

        #region MessageConventionsForNonNSB

        var conventions = endpointConfiguration.Conventions();
        conventions.DefiningEventsAs(type => type == typeof(StockTick));

        #endregion

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
        var rand = new Random();

        Console.WriteLine("Press any key to start publishing");
        Console.ReadKey(true);

        do
        {
            while (!Console.KeyAvailable)
            {
                var stockSymbol = Symbols[rand.Next(0, Symbols.Length - 1)];

                var stockTick = new StockTick
                {
                    Symbol = stockSymbol,
                    Timestamp = DateTime.UtcNow
                };
                await endpointInstance.Publish(stockTick)
                    .ConfigureAwait(false);

                await Task.Delay(500)
                    .ConfigureAwait(false);

                Console.WriteLine($"Published StockTick Event with Symbol {stockSymbol}. Press escape to stop publishing events.");
            }
        } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
    }

    static string[] Symbols =
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