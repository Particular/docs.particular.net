using System;
using System.Collections.Generic;
using NServiceBus;
using NServiceBus.Logging;

public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
{
    static ILog log = LogManager.GetLogger(typeof(PlaceOrderHandler));
    static List<Guid> wasMessageDelayed = new List<Guid>();
    IBus bus;

    public PlaceOrderHandler(IBus bus)
    {
        this.bus = bus;
    }

    #region PlaceOrderHandler
    public void Handle(PlaceOrder message)
    {
        if (ShouldMessageBeDelayed(message.Id))
        {
            bus.Defer(TimeSpan.FromSeconds(5), message);
            log.Info($"[Defer Message Handling] Deferring Message with Id: {message.Id}");
            return;
        }

        log.Info($"[Defer Message Handling] Order for Product:{message.Product} placed with id: {message.Id}");
    }
    #endregion

    bool ShouldMessageBeDelayed(Guid id)
    {
        if (wasMessageDelayed.Contains(id))
        {
            return false;
        }

        wasMessageDelayed.Add(id);
        return true;
    }
}