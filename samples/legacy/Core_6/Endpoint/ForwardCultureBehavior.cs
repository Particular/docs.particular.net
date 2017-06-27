using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;
using NServiceBus.Transport;

class ForwardCultureBehavior : Behavior<IOutgoingLogicalMessageContext>
{
    public override Task Invoke(IOutgoingLogicalMessageContext context, Func<Task> next)
    {
        IncomingMessage incomingMessage;
        string incomingCulture;
        if (context.TryGetIncomingPhysicalMessage(out incomingMessage) 
            && incomingMessage.Headers.TryGetValue("Culture", out incomingCulture))
        {
            context.Headers["Culture"] = incomingCulture;
        }
        return next();
    }
}