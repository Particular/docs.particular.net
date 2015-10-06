using System;
using System.Threading.Tasks;
using NServiceBus.OutgoingPipeline;
using NServiceBus.Pipeline;
using NServiceBus.TransportDispatch;

#region outgoing-header-behavior

class OutgoingHeaderBehavior : Behavior<OutgoingPhysicalMessageContext>
{
    public override async Task Invoke(OutgoingPhysicalMessageContext context, Func<Task> next)
    {
        context.SetHeader("OutgoingHeaderBehavior", "ValueOutgoingHeaderBehavior");
        await next();
    }
}

#endregion