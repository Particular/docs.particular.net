using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;
using NServiceBus.Transport;

class ForwardUserBehavior : Behavior<IOutgoingLogicalMessageContext>
{
    public override Task Invoke(IOutgoingLogicalMessageContext context, Func<Task> next)
    {
        IncomingMessage incomingMessage;
        string incomingUser;
        if (context.TryGetIncomingPhysicalMessage(out incomingMessage)
            && incomingMessage.Headers.TryGetValue("User", out incomingUser))
        {
            context.Headers["User"] = incomingUser;
        }
        return next();
    }
}