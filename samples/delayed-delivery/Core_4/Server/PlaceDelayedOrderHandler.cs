using NServiceBus;
using NServiceBus.Logging;

#region PlaceDelayedOrderHandler

public class PlaceDelayedOrderHandler :
    IHandleMessages<PlaceDelayedOrder>
{
    static ILog log = LogManager.GetLogger(typeof(PlaceDelayedOrderHandler));

    public void Handle(PlaceDelayedOrder message)
    {
        log.Info($"[Defer Message Delivery] Order for Product:{message.Product} placed with id: {message.Id}");
    }
}

#endregion
