using System;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;

#region incoming-header-behavior
class IncomingHeaderBehavior :
    IBehavior<IncomingContext>
{
    public void Invoke(IncomingContext context, Action next)
    {
        var headers = context.PhysicalMessage.Headers;
        headers.Add("IncomingHeaderBehavior", "ValueIncomingHeaderBehavior");
        next();
    }
}

#endregion