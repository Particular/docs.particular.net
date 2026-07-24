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
            if (incomingMessage.Headers.TryGetValue("CustomerId", out var incomingCustomerId))
            {
                context.Headers["CustomerId"] = incomingCustomerId;
            }
        }

        return next();
    }
}
#endregion
