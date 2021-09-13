using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

#region outgoing-header-behavior

class OutgoingHeaderBehavior :
    Behavior<IOutgoingPhysicalMessageContext>
{
    public override Task Invoke(IOutgoingPhysicalMessageContext context, Func<Task> next)
    {
        var headers = context.Headers;
        headers["OutgoingHeaderBehavior"] = "ValueOutgoingHeaderBehavior";
        return next();
    }
}

#endregion