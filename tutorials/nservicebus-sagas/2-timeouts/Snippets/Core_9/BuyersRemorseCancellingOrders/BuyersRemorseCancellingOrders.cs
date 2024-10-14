using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace Core_9.BuyersRemorseCancellingOrders;

class UICommands(ILogger<UICommands> logger)
{
    async Task Execute(IMessageSession endpointInstance)
    {
        #region BuyersRemorseCancellingOrders
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
                    await endpointInstance.Send(command);

                    lastOrder = command.OrderId; // Store order identifier to cancel if needed.
                    break;

                case ConsoleKey.C:
                    var cancelCommand = new CancelOrder
                    {
                        OrderId = lastOrder
                    };
                    await endpointInstance.Send(cancelCommand);
                    logger.LogInformation("Sent a CancelOrder command, {OrderId}", cancelCommand.OrderId);
                    break;

                case ConsoleKey.Q:
                    return;

                default:
                    logger.LogInformation("Unknown input. Please try again.");
                    break;
            }
        }
        #endregion
    }
}

internal class PlaceOrder
{
    public string OrderId { get; set; }
}

internal class CancelOrder
{
    public string OrderId { get; set; }
}