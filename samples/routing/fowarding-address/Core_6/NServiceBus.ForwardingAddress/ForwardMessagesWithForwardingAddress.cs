using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

#region forward-matching-messages-behavior

class ForwardMessagesWithForwardingAddress : ForkConnector<IIncomingPhysicalMessageContext, IRoutingContext>
{
    public override async Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next,
        Func<IRoutingContext, Task> fork)
    {
        var state = new MessageForwardingState();
        context.Extensions.Set(state);

        await next().ConfigureAwait(false);

        var forwardingRoutes = state.GetRoutingStrategies();

        if (forwardingRoutes.Any())
        {
            await fork(context.CreateRoutingContext(forwardingRoutes)).ConfigureAwait(false);
        }
    }
}

#endregion