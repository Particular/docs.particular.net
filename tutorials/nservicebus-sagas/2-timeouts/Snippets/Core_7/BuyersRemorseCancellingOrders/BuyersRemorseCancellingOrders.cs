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
            #region BuyersRemorseCancellingOrders
            switch (parts[0].ToLowerInvariant())
            {
                case "place":
                    // ... 
                    break;
                case "cancel":
                    {
                        var command = new CancelOrder
                        {
                            OrderId = orderId
                        };
                        await endpointInstance.Send(command)
                            .ConfigureAwait(false);
                        log.Info($"Sent a correlated message to {orderId}");
                        break;
                    }
            }
            #endregion
        }
    }
    internal class CancelOrder
    {
        public string OrderId { get; set; }
    }
}