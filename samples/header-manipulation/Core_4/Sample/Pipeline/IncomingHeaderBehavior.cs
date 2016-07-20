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
        var headers = context.PhysicalMessage.Headers;
        headers.Add("IncomingHeaderBehavior", "ValueIncomingHeaderBehavior");
        next();
    }
}

#endregion