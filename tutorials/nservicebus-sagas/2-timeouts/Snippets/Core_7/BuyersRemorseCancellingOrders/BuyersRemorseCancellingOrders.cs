namespace Core_7.BuyersRemorseCancellingOrders
{
    using NServiceBus;
    using NServiceBus.Logging;
    using System;
    using System.Threading.Tasks;

    class UICommands
    {
        static ILog log = LogManager.GetLogger<UICommands>();

        async Task Execute(IMessageSession endpointInstance)
        {
            #region BuyersRemorseCancellingOrders
            var lastOrder = string.Empty;

            while (true)
            {
                log.Info("Press 'P' to place an order, 'C' to cancel last order, or 'Q' to quit.");
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
                        log.Info($"Sending PlaceOrder command, OrderId = {command.OrderId}");
                        await endpointInstance.Send(command)
                            .ConfigureAwait(false);

                        lastOrder = command.OrderId; // Store order identifier to cancel if needed.
                        break;

                    case ConsoleKey.C:
                        var cancelCommand = new CancelOrder
                        {
                            OrderId = lastOrder
                        };
                        await endpointInstance.Send(cancelCommand)
                            .ConfigureAwait(false);
                        log.Info($"Sent a correlated message to {cancelCommand.OrderId}");
                        break;

                    case ConsoleKey.Q:
                        return;

                    default:
                        log.Info("Unknown input. Please try again.");
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
}