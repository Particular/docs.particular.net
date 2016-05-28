using System;
using NServiceBus;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;

#pragma warning disable 618

#region outgoing-header-behavior
public class OutgoingHeaderBehavior : IBehavior<SendPhysicalMessageContext>
{
    IBus bus;

    public OutgoingHeaderBehavior(IBus bus)
    {
        this.bus = bus;
    }

    public void Invoke(SendPhysicalMessageContext context, Action next)
    {
        var incomingContext = bus.CurrentMessageContext;
        var incomingMessageId = incomingContext?.Headers["NServiceBus.MessageId"];

        context.MessageToSend
            .Headers
            .Add("OutgoingHeaderBehavior", "ValueOutgoingHeaderBehavior");
        next();
    }
}
#endregion