using NServiceBus.Extensibility;
using NServiceBus.ObjectBuilder;
using NServiceBus.Pipeline;
using NServiceBus.Routing;
using NServiceBus.Transport;
using System.Collections.Generic;

class ForwardedRoutingContext : IRoutingContext
{
    public ForwardedRoutingContext(OutgoingMessage message, RoutingStrategy forwardingRoutingStrategy, IBehaviorContext innerContext)
    {
        Message = message;
        RoutingStrategies = new[] { forwardingRoutingStrategy };
        Builder = innerContext.Builder;
        Extensions = innerContext.Extensions;
    }

    public OutgoingMessage Message { get; }

    public IReadOnlyCollection<RoutingStrategy> RoutingStrategies { get; set; }

    public IBuilder Builder { get; }

    public ContextBag Extensions { get; }
}