using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using Store.Messages.Events;

class OrderAcceptedHandler : IHandleMessages<OrderAccepted>
{

    public async Task Handle(OrderAccepted message, IMessageHandlerContext context)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        Console.WriteLine("Customer: {0} is now a preferred customer publishing for other service concerns", message.ClientId);

        // publish this event as an asynchronous event
        await context.Publish<ClientBecamePreferred>(m =>
        {
            m.ClientId = message.ClientId;
            m.PreferredStatusExpiresOn = DateTime.Now.AddMonths(2);
        });
    }
    
}