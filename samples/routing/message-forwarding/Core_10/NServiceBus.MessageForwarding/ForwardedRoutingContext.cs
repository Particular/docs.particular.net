using NServiceBus.Extensibility;
using NServiceBus.Pipeline;
using NServiceBus.Routing;
using NServiceBus.Transport;
using System;
using System.Collections.Generic;
using System.Threading;

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

    public ContextBag Extensions { get; }

    public CancellationToken CancellationToken { get; }

    public IServiceProvider Builder { get; }
}