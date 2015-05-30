using System;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;

#pragma warning disable 618

#region outgoing-header-behavior
public class OutgoingHeaderBehavior : IBehavior<SendPhysicalMessageContext>
{
    public void Invoke(SendPhysicalMessageContext context, Action next)
    {
        context.MessageToSend
            .Headers
            .Add("KeyFromOutgoingHeaderBehavior", "ValueFromOutgoingHeaderBehavior");
        next();
    }
}
#endregion