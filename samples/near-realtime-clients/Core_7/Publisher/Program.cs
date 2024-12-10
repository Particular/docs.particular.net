using System;
using System.Threading.Tasks;
using NServiceBus;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Publisher";
        var endpointConfiguration = new EndpointConfiguration("Samples.NearRealTimeClients.Publisher");
        endpointConfiguration.UseTransport<LearningTransport>();

        endpointConfiguration.SendFailedMessagesTo("error");

        #region MessageConventionsForNonNSB

        var conventions = endpointConfiguration.Conventions();
        conventions.DefiningEventsAs(type => type == typeof(StockTick));

        #endregion

        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        await Start(endpointInstance);
        await endpointInstance.Stop();
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
                await endpointInstance.Publish(stockTick);

                await Task.Delay(500);

                Console.WriteLine($"Published StockTick Event with Symbol {stockSymbol}. Press escape to stop publishing events.");
            }
        } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
    }

    static readonly string[] Symbols =
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
