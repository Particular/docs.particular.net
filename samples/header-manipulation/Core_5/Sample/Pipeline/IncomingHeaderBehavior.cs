using System;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;

#region incoming-header-behavior
class IncomingHeaderBehavior : IBehavior<IncomingContext>
{
    public void Invoke(IncomingContext context, Action next)
    {
        context.PhysicalMessage
            .Headers
            .Add("IncomingHeaderBehavior", "ValueIncomingHeaderBehavior");
        next();
    }
}

#endregion