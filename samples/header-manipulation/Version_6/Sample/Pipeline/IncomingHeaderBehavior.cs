using System;
using NServiceBus;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;

#region incoming-header-behavior
class IncomingHeaderBehavior : PhysicalMessageProcessingStageBehavior
{
    public override void Invoke(Context context, Action next)
    {
        context.Get<TransportMessage>()
            .Headers
            .Add("IncomingHeaderBehavior", "ValueIncomingHeaderBehavior");
        next();
    }
}

#endregion