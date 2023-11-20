using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

#region incoming-header-behavior
class IncomingHeaderBehavior :
    Behavior<IIncomingPhysicalMessageContext>
{
    public override Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
    {
        var headers = context.Message.Headers;
        headers.Add("IncomingHeaderBehavior", "ValueIncomingHeaderBehavior");
        return next();
    }
}

#endregion