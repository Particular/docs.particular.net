using System.Threading.Tasks;
using System.Threading;
using System;
using Messages;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Microsoft.Extensions.Hosting;

namespace ClientUI;

public class InputLoopService(IMessageSession messageSession) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (true)
        {
            Console.WriteLine("Press 'P' to place an order, or 'Q' to quit.");
            var key = Console.ReadKey();
            Console.WriteLine();

            switch (key.Key)
            {
                case ConsoleKey.P:
                    // Instantiate the command
                    var command = new PlaceOrder
                    {
                        OrderId = Guid.NewGuid().ToString()
                    };

                    // Send the command
                    Console.WriteLine($"Sending PlaceOrder command, OrderId = {command.OrderId}");
                    await messageSession.Send(command, cancellationToken: stoppingToken);

                    break;

                case ConsoleKey.Q:
                    return;

                default:
                    Console.WriteLine("Unknown input. Please try again.");
                    break;
            }
        }
    }
}