using System;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;

#pragma warning disable 618

#region incoming-header-behavior

public class IncomingHeaderBehavior :
    IBehavior<ReceivePhysicalMessageContext>
{
    public void Invoke(ReceivePhysicalMessageContext context, Action next)
    {
        context.PhysicalMessage
            .Headers
            .Add("IncomingHeaderBehavior", "ValueIncomingHeaderBehavior");
        next();
    }
}

#endregion