using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

#region behavior
class PropagateCustomerIdHeaderBehavior : Behavior<IOutgoingLogicalMessageContext>
{
    public override Task Invoke(IOutgoingLogicalMessageContext context, Func<Task> next)
    {
        if (context.TryGetIncomingPhysicalMessage(out var incomingMessage))
        {
            if (incomingMessage.Headers.TryGetValue("CustomerId", out var incomingConversationId))
            {
                context.Headers["CustomerId"] = incomingConversationId;
            }
        }

        return next();
    }
}
#endregion
