using System;
using NServiceBus;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;

#region outgoing-header-behavior

class OutgoingHeaderBehavior : IBehavior<OutgoingContext>
{
    IBus bus;

    public OutgoingHeaderBehavior(IBus bus)
    {
        this.bus = bus;
    }

    public void Invoke(OutgoingContext context, Action next)
    {
        IMessageContext incomingContext = bus.CurrentMessageContext;
        if (incomingContext != null)
        {
            string incomingMessageId = incomingContext.Headers["NServiceBus.MessageId"];
        }

        context.OutgoingMessage
            .Headers
            .Add("OutgoingHeaderBehavior", "ValueOutgoingHeaderBehavior");
        next();
    }
}

#endregion