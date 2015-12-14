using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Pipeline;

#region incoming-header-behavior
class IncomingHeaderBehavior : Behavior<IncomingPhysicalMessageContext>
{
    public override async Task Invoke(IncomingPhysicalMessageContext context, Func<Task> next)
    {
        context.Message
            .Headers
            .Add("IncomingHeaderBehavior", "ValueIncomingHeaderBehavior");
        await next();
    }
}

#endregion