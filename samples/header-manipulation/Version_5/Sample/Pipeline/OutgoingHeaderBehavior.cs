using System;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;

#region outgoing-header-behavior

class OutgoingHeaderBehavior : IBehavior<OutgoingContext>
{
    public void Invoke(OutgoingContext context, Action next)
    {
        context.OutgoingMessage
            .Headers
            .Add("KeyFromOutgoingHeaderBehavior", "ValueFromOutgoingHeaderBehavior");
        next();
    }
}

#endregion