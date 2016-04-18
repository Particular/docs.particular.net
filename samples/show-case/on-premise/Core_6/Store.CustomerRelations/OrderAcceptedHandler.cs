using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Store.Messages.Events;

class OrderAcceptedHandler : IHandleMessages<OrderAccepted>
{
    static ILog log = LogManager.GetLogger<OrderAcceptedHandler>();

    public async Task Handle(OrderAccepted message, IMessageHandlerContext context)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        log.InfoFormat("Customer: {0} is now a preferred customer publishing for other service concerns", message.ClientId);

        // publish this event as an asynchronous event
        await context.Publish<ClientBecamePreferred>(m =>
        {
            m.ClientId = message.ClientId;
            m.PreferredStatusExpiresOn = DateTime.Now.AddMonths(2);
        });
    }
    
}