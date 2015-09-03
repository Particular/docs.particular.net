using System;
using NServiceBus;

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