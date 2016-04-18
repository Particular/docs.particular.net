using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

#region outgoing-header-behavior

class OutgoingHeaderBehavior : Behavior<IOutgoingPhysicalMessageContext>
{
    public override async Task Invoke(IOutgoingPhysicalMessageContext context, Func<Task> next)
    {
        context.Headers["OutgoingHeaderBehavior"] = "ValueOutgoingHeaderBehavior";
        await next();
    }
}

#endregion