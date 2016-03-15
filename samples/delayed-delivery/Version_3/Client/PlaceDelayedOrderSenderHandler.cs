using System;
using NServiceBus;

public class PlaceDelayedOrderSenderHandler : IHandleMessages<PlaceDelayedOrder>
{
    IBus bus;

    public PlaceDelayedOrderSenderHandler(IBus bus)
    {
        this.bus = bus;
    }

    #region PlaceDelayedOrderSenderHandler
    public void Handle(PlaceDelayedOrder message)
    {
        bus.Send("Samples.DelayedDelivery.Server", message);

        Console.WriteLine("[Defer Message Delivery] Sent a new PlaceDelayedOrder message with id: {0}", message.Id.ToString("N"));
    }
    #endregion
}
