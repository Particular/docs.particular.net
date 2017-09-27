using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus.Logging;
using NServiceBus.Pipeline;
using NServiceBus.Routing;

#region set-forwarding-address-behavior

class RerouteMessagesWithForwardingAddress : Behavior<IIncomingLogicalMessageContext>
{
    public override Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
    {
        var messageForwardingState = context.Extensions.Get<MessageForwardingState>();

        foreach (var forwardingRoute in forwardingRoutesLookup[context.Message.MessageType])
        {
            Log.InfoFormat("Message {0} has forwarding address to {1}", context.MessageId, forwardingRoute);

            var routingStrategy = ResolveForwardingRoute(context, forwardingRoute);

            messageForwardingState.AddRoutingStrategy(routingStrategy);

            context.MessageHandled = true;
        }

        return next();
    }

    UnicastRoutingStrategy ResolveForwardingRoute(IIncomingLogicalMessageContext context, UnicastRoute forwardingRoute)
    {
        var outgoingLogicalMessage = new OutgoingLogicalMessage(
            context.Message.MessageType,
            context.Message.Instance);

        var routingStrategy = routeResolver.ResolveRoute(
            forwardingRoute,
            outgoingLogicalMessage,
            context.MessageId,
            context.Headers,
            context.Extensions);

        return routingStrategy;
    }

    public RerouteMessagesWithForwardingAddress(ILookup<Type, UnicastRoute> forwardingRoutesLookup, UnicastRouteResolver routeResolver)
    {
        this.forwardingRoutesLookup = forwardingRoutesLookup;
        this.routeResolver = routeResolver;
    }

    ILookup<Type, UnicastRoute> forwardingRoutesLookup;
    UnicastRouteResolver routeResolver;

    static ILog Log = LogManager.GetLogger<RerouteMessagesWithForwardingAddress>();
}

#endregion