using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.DelayedDelivery;
using NServiceBus.DeliveryConstraints;
using NServiceBus.Extensibility;
using NServiceBus.Pipeline;
using NServiceBus.Routing;
using NServiceBus.Settings;

public class RerouteDelayedMessagesBehavior : Behavior<IRoutingContext>
{
    string delayQueue;
    string localAddress;

    public RerouteDelayedMessagesBehavior(ReadOnlySettings settings)
    {
        delayQueue = settings.Get<string>("ASBDelayQueue");
        localAddress = settings.LocalAddress();
    }

    public override Task Invoke(IRoutingContext context, Func<Task> next)
    {
        if (!IsDeferred(context))
        {
            return next();
        }

        var newRoutingStrategies = context.RoutingStrategies.Select(s => RerouteToDelayQueue(s, context));
        context.RoutingStrategies = newRoutingStrategies.ToArray();

        return next();
    }

    RoutingStrategy RerouteToDelayQueue(RoutingStrategy routingStrategy, IRoutingContext context)
    {
        var headers = new Dictionary<string, string>(context.Message.Headers);
        var originalTag = routingStrategy.Apply(headers);
        if (!(originalTag is UnicastAddressTag unicastTag))
        {
            return routingStrategy;
        }
        if (unicastTag.Destination != localAddress)
        {
            return routingStrategy;
        }
        return new UnicastRoutingStrategy(delayQueue);
    }

    static bool IsDeferred(IExtendable context)
    {
        if (context.Extensions.TryGetDeliveryConstraint(out DoNotDeliverBefore _)
            || context.Extensions.TryGetDeliveryConstraint(out DelayDeliveryWith _))
        {
            return true;
        }
        return false;
    }
}