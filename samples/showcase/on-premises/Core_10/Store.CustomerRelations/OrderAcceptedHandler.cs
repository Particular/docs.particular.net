using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;
using Store.Messages.Events;

class OrderAcceptedHandler(ILogger<OrderAcceptedHandler> logger) :
    IHandleMessages<OrderAccepted>
{
    public Task Handle(OrderAccepted message, IMessageHandlerContext context)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        logger.LogInformation("Customer: {ClientId} is now a preferred customer publishing for other service concerns", message.ClientId);

        // publish this event as an asynchronous event
        var clientBecamePreferred = new ClientBecamePreferred
        {
            ClientId = message.ClientId,
            PreferredStatusExpiresOn = DateTime.Now.AddMonths(2)
        };
        return context.Publish(clientBecamePreferred);
    }
}