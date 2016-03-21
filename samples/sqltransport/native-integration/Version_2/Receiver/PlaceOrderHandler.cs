using NServiceBus;
using NServiceBus.Logging;

public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
{
    static ILog log = LogManager.GetLogger<PlaceOrderHandler>();
    public void Handle(PlaceOrder message)
    {
        log.InfoFormat("Order {0} placed", message.OrderId);
    }
}