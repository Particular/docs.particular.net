using NServiceBus;
using NServiceBus.Logging;

public class OrderPlacedHandler : IHandleMessages<OrderPlaced>
{
    static ILog log = LogManager.GetLogger<OrderPlacedHandler>();

    public void Handle(OrderPlaced orderPlaced)
    {
        log.InfoFormat("Received OrderPlaced. OrderId: {0}. Worker: {1}", orderPlaced.OrderId, orderPlaced.WorkerName);
    }
}
