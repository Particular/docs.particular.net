using NServiceBus;
using NServiceBus.Logging;

public class PlaceDelayedOrderSenderHandler :
    IHandleMessages<PlaceDelayedOrder>
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
        log.Info($"[Defer Message Delivery] Sent a PlaceDelayedOrder message with id: {message.Id.ToString("N")}");
    }
    #endregion
}