using System;
using System.Collections.Generic;
using System.Linq;
using NServiceBus;
using NServiceBus.Extensibility;
using NServiceBus.Pipeline;
using NServiceBus.Routing;
using NServiceBus.Transport;

// NOTE: This class probably belongs in the core. This logic is taken from the UnicastSendRouter which goes from MessageType -> UnicastRoutingStrategy
class UnicastRouteResolver
{
    public UnicastRouteResolver(ITransportAddressResolver transportAddressResolver, EndpointInstances endpointInstances, IDistributionPolicy distributionPolicy)
    {
        this.transportAddressResolver = transportAddressResolver;
        this.endpointInstances = endpointInstances;
        this.distributionPolicy = distributionPolicy;
    }

    public UnicastRoutingStrategy ResolveRoute(UnicastRoute route, OutgoingLogicalMessage outgoingLogicalMessage, string messageId, Dictionary<string, string> headers, ContextBag contextBag)
    {
        if (route.PhysicalAddress != null)
        {
            return new UnicastRoutingStrategy(route.PhysicalAddress);
        }
        if (route.Instance != null)
        {
            return new UnicastRoutingStrategy(TranslateTransportAddress(route.Instance));
        }
        var instances = endpointInstances.FindInstances(route.Endpoint).Select(e => TranslateTransportAddress(e)).ToArray();
        var distributionContext = new DistributionContext(instances, outgoingLogicalMessage, messageId, headers, transportAddressResolver, contextBag);
        var selectedInstanceAddress = distributionPolicy.GetDistributionStrategy(route.Endpoint, DistributionStrategyScope.Send).SelectDestination(distributionContext);
        return new UnicastRoutingStrategy(selectedInstanceAddress);
    }

    string TranslateTransportAddress(EndpointInstance instance) =>
        transportAddressResolver.ToTransportAddress(new QueueAddress(instance.Endpoint, instance.Discriminator, instance.Properties));

    ITransportAddressResolver transportAddressResolver;
    EndpointInstances endpointInstances;
    IDistributionPolicy distributionPolicy;

}