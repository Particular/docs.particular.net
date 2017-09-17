using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Store.Messages.Events;

class OrderAcceptedHandler :
    IHandleMessages<OrderAccepted>
{
    static ILog log = LogManager.GetLogger<OrderAcceptedHandler>();

    public Task Handle(OrderAccepted message, IMessageHandlerContext context)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        log.Info($"Customer: {message.ClientId} is now a preferred customer publishing for other service concerns");

        // publish this event as an asynchronous event
        var clientBecamePreferred = new ClientBecamePreferred
        {
            ClientId = message.ClientId,
            PreferredStatusExpiresOn = DateTime.Now.AddMonths(2)
        };
        return context.Publish(clientBecamePreferred);
    }
}