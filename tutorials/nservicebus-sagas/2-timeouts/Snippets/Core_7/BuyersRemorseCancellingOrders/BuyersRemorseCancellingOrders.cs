namespace Core_7.BuyersRemorseCancellingOrders
{
    using NServiceBus;
    using NServiceBus.Logging;
    using System.Threading.Tasks;

    class UICommands
    {
        static ILog log = LogManager.GetLogger<UICommands>();

        async Task Execute(IMessageSession endpointInstance, string orderId, string[] parts)
        {
            var key = Console.ReadKey();
            #region BuyersRemorseCancellingOrders
            log.Info("Press 'P' to place an order, 'C' to cancel last order, or 'Q' to quit.");
            var key = Console.ReadKey();
                Console.WriteLine();
            var lastOrder = string.Empty;
            switch (key.Key)
            {
                case ConsoleKey.P:
                    // ... 
                    lastOrder = command.OrderId;  // Store order identifier to cancel if needed.
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

            }
            #endregion
        }
    }
    internal class CancelOrder
    {
        public string OrderId { get; set; }
    }
}