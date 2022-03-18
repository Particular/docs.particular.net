using System;
using System.Collections.Generic;
using System.Threading;
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
            context.Builder,
            context.CancellationToken,
            context);
    }

    class ForwardingRoutingContext :
        ContextBag,
        IRoutingContext
    {
        public ForwardingRoutingContext(
            OutgoingMessage message,
            IReadOnlyCollection<RoutingStrategy> routingStrategies,
            IServiceProvider builder,
            CancellationToken cancellationToken,
            IBehaviorContext parentContext) : base(parentContext?.Extensions)
        {
            Message = message;
            RoutingStrategies = routingStrategies;
            Builder = builder;
            CancellationToken = cancellationToken;
        }

        public IServiceProvider Builder { get; }
        public ContextBag Extensions => this;
        public OutgoingMessage Message { get; }
        public IReadOnlyCollection<RoutingStrategy> RoutingStrategies { get; set; }
        public CancellationToken CancellationToken { get; }
    }
}