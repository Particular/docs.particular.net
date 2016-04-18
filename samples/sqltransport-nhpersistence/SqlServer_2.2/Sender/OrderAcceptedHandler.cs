using NServiceBus;
using NServiceBus.Logging;

public class OrderAcceptedHandler : IHandleMessages<OrderAccepted>
{
    static ILog log = LogManager.GetLogger<OrderAcceptedHandler>();
    public void Handle(OrderAccepted message)
    {
        log.InfoFormat("Order {0} accepted.", message.OrderId);
    }
}