using System;
using System.Collections.Generic;
using log4net;
using NServiceBus;

public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
{
    static List<Guid> wasMessageDelayed = new List<Guid>();
    static ILog log = LogManager.GetLogger(typeof(PlaceOrderHandler));
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
            log.InfoFormat(@"[Defer Message Handling] Deferring Message with Id: {0}", message.Id);
            return;
        }

        log.InfoFormat(@"[Defer Message Handling] Order for Product:{0} placed with id: {1}", message.Product, message.Id);
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
