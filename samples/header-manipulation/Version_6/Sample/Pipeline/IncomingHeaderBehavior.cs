using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Pipeline;

#region incoming-header-behavior
class IncomingHeaderBehavior : Behavior<PhysicalMessageProcessingContext>
{
    public override async Task Invoke(PhysicalMessageProcessingContext context, Func<Task> next)
    {
        context.Message
            .Headers
            .Add("IncomingHeaderBehavior", "ValueIncomingHeaderBehavior");
        await next();
    }
}

#endregion