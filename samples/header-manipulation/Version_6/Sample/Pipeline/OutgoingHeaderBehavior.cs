using System;
using NServiceBus.OutgoingPipeline;
using NServiceBus.TransportDispatch;

#region outgoing-header-behavior

class OutgoingHeaderBehavior : PhysicalOutgoingContextStageBehavior
{
    public override void Invoke(Context context, Action next)
    {
        context.SetHeader("OutgoingHeaderBehavior", "ValueOutgoingHeaderBehavior");
        next();
    }
}

#endregion