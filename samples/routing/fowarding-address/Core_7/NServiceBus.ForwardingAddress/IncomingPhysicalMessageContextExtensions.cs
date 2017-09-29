using System.Collections.Generic;
using NServiceBus.Extensibility;
using NServiceBus.ObjectBuilder;
using NServiceBus.Pipeline;
using NServiceBus.Routing;
using NServiceBus.Transport;

static class IncomingPhysicalMessageContextExtensions
{
    public static IRoutingContext CreateRoutingContext(
        this IIncomingPhysicalMessageContext context,
        IReadOnlyCollection<RoutingStrategy> routingStrategies)
    {
        return new ForwardingRoutingContext(
            new OutgoingMessage(context.MessageId, context.Message.Headers, context.Message.Body),
            routingStrategies,
            context
        );
    }

    class ForwardingRoutingContext :
        ContextBag,
        IRoutingContext
    {
        public ForwardingRoutingContext(OutgoingMessage message,
            IReadOnlyCollection<RoutingStrategy> routingStrategies,
            IBehaviorContext parentContext) : base(parentContext?.Extensions)
        {
            Message = message;
            RoutingStrategies = routingStrategies;
        }

        public IBuilder Builder => Get<IBuilder>();
        public ContextBag Extensions => this;
        public OutgoingMessage Message { get; }
        public IReadOnlyCollection<RoutingStrategy> RoutingStrategies { get; set; }
    }
}