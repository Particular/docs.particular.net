using NServiceBus.Pipeline;
using NServiceBus.Routing;
using NServiceBus.Transport;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#region forward-processed-messages-behavior
class ForwardProcessedMessagesBehavior : ForkConnector<IIncomingPhysicalMessageContext, IRoutingContext>
{
    private string forwardingAddress;

    public ForwardProcessedMessagesBehavior(string forwardingAddress)
    {
        this.forwardingAddress = forwardingAddress;
    }

    public override async Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next, Func<IRoutingContext, Task> fork)
    {
        var messageToForward = new OutgoingMessage(
            context.Message.MessageId,
            new Dictionary<string, string>(context.Message.Headers),
            context.Message.Body);

        await next()
            .ConfigureAwait(false);

        var forwardingRoutingStrategy = new UnicastRoutingStrategy(forwardingAddress);

        var routingContext = new ForwardedRoutingContext(
            messageToForward,
            forwardingRoutingStrategy,
            context);

        await fork(routingContext)
            .ConfigureAwait(false);
    }
}
#endregion