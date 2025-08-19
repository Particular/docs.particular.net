using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "Publisher";
var builder = Host.CreateApplicationBuilder(args);
var endpointConfiguration = new EndpointConfiguration("Samples.NearRealTimeClients.Publisher");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

endpointConfiguration.SendFailedMessagesTo("error");

#region MessageConventionsForNonNSB

var conventions = endpointConfiguration.Conventions();
conventions.DefiningEventsAs(type => type == typeof(StockTick));

#endregion

endpointConfiguration.EnableInstallers();

Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();
var rand = new Random();

Console.WriteLine("Press any key to start publishing");
Console.ReadKey(true);

string[] symbols =
[
    "MSFT",
    "AAPL",
    "GOOGL",
    "ORCL",
    "INTC",
    "HPQ",
    "CSCO"
];

do
{
    while (!Console.KeyAvailable)
    {
        var stockSymbol = symbols[rand.Next(0, symbols.Length - 1)];

        var stockTick = new StockTick
        {
            Symbol = stockSymbol,
            Timestamp = DateTime.UtcNow
        };
        await messageSession.Publish(stockTick);

        await Task.Delay(500);

        Console.WriteLine($"Published StockTick Event with Symbol {stockSymbol}. Press escape to stop publishing events.");
    }
} while (Console.ReadKey(true).Key != ConsoleKey.Escape);

await host.StopAsync();
