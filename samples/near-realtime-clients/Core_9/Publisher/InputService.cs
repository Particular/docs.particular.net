using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Routing;

namespace Publisher
{
    public class InputService(IMessageSession messageSession) : BackgroundService
    {
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
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
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
                    await messageSession.Publish(stockTick);

                    await Task.Delay(500);

                    Console.WriteLine($"Published StockTick Event with Symbol {stockSymbol}. Press escape to stop publishing events.");
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

        }
    }
}
