using System;
using System.Threading.Tasks;
using NServiceBus.OutgoingPipeline;
using NServiceBus.Pipeline;

#region outgoing-header-behavior

class OutgoingHeaderBehavior : Behavior<OutgoingPhysicalMessageContext>
{
    public override async Task Invoke(OutgoingPhysicalMessageContext context, Func<Task> next)
    {
        context.Headers["OutgoingHeaderBehavior"] = "ValueOutgoingHeaderBehavior";
        await next();
    }
}

#endregion