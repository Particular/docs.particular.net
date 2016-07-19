using NServiceBus;
using NServiceBus.Logging;

#region sender-event-handler

public class OrderPlacedHandler :
    IHandleMessages<OrderPlaced>
{
    static ILog log = LogManager.GetLogger<OrderPlacedHandler>();

    public void Handle(OrderPlaced orderPlaced)
    {
        log.Info($"Received OrderPlaced. OrderId: {orderPlaced.OrderId}. Worker: {orderPlaced.WorkerName}");
    }
}

#endregion
