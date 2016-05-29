using System;
using System.Diagnostics;
using NServiceBus;
using NServiceBus.Logging;
using Store.Messages.Events;

class OrderAcceptedHandler : IHandleMessages<OrderAccepted>
{
    static ILog log = LogManager.GetLogger<OrderAcceptedHandler>();
    IBus bus;

    public OrderAcceptedHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(OrderAccepted message)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        log.InfoFormat("Customer: {0} is now a preferred customer publishing for other service concerns", message.ClientId);

        // publish this event as an asynchronous event
        var clientBecamePreferred = new ClientBecamePreferred
        {
            ClientId = message.ClientId,
            PreferredStatusExpiresOn = DateTime.Now.AddMonths(2)
        };
        bus.Publish(clientBecamePreferred);
    }
}