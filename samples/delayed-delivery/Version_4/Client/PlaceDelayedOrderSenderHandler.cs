using NServiceBus;
using NServiceBus.Logging;

public class PlaceDelayedOrderSenderHandler : IHandleMessages<PlaceDelayedOrder>
{
    static ILog log = LogManager.GetLogger(typeof(PlaceDelayedOrderSenderHandler));
    IBus bus;

    public PlaceDelayedOrderSenderHandler(IBus bus)
    {
        this.bus = bus;
    }

    #region PlaceDelayedOrderSenderHandler
    public void Handle(PlaceDelayedOrder message)
    {
        bus.Send("Samples.DelayedDelivery.Server", message);

        log.InfoFormat("[Defer Message Delivery] Sent a new PlaceDelayedOrder message with id: {0}", message.Id.ToString("N"));
    }
    #endregion
}