using Messages;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ClientUI;

public class InputLoopService(IMessageSession messageSession,ILogger<InputLoopService> logger) : BackgroundService
{    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var lastOrder = string.Empty;

        while (true)
        {
            Console.Title = "ClientUI";
          
            logger.LogInformation("Press 'P' to place an order, 'C' to cancel an order, or 'Q' to quit.");
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
                    logger.LogInformation("Sending PlaceOrder command, OrderId = {OrderId}", command.OrderId);
                    await messageSession.Send(command);

                    lastOrder = command.OrderId; // Store order identifier to cancel if needed.
                    break;
                case ConsoleKey.C:
                    var cancelCommand = new CancelOrder
                    {
                        OrderId = lastOrder
                    };
                    await messageSession.Send(cancelCommand);
                    logger.LogInformation("Sent a CancelOrder command, {OrderId}", cancelCommand.OrderId);
                    break;

                case ConsoleKey.Q:
                    return;

                default:
                    logger.LogInformation("Unknown input. Please try again.");
                    break;
            }
        }
    }
}
