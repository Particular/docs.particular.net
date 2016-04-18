#region PlaceDelayedOrderHandler
using log4net;
using NServiceBus;

public class PlaceDelayedOrderHandler : IHandleMessages<PlaceDelayedOrder>
{
    static ILog log = LogManager.GetLogger(typeof(PlaceDelayedOrderHandler));

    public void Handle(PlaceDelayedOrder message)
    {
        log.InfoFormat(@"[Defer Message Delivery] Order for Product:{0} placed with id: {1}", message.Product, message.Id);
    }
}

#endregion