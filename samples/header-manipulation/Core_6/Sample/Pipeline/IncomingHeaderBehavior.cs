using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

#region incoming-header-behavior
class IncomingHeaderBehavior :
    Behavior<IIncomingPhysicalMessageContext>
{
    public override Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
    {
        context.Message
            .Headers
            .Add("IncomingHeaderBehavior", "ValueIncomingHeaderBehavior");
        return next();
    }
}

#endregion