using System;
using System.Collections.Generic;
using System.Linq;
using NServiceBus;
using NServiceBus.Extensibility;
using NServiceBus.Pipeline;
using NServiceBus.Routing;

// NOTE: This class probably belongs in the core. This logic is taken from the UnicastSendRouter which goes from MessageType -> UnicastRoutingStrategy
class UnicastRouteResolver
{
    public UnicastRoutingStrategy ResolveRoute(UnicastRoute route, OutgoingLogicalMessage outgoingLogicalMessage, string messageId, Dictionary<string, string> headers, ContextBag contextBag)
    {
        if (route.PhysicalAddress != null)
        {
            return new UnicastRoutingStrategy(route.PhysicalAddress);
        }
        if (route.Instance != null)
        {
            return new UnicastRoutingStrategy(transportAddressTranslation(route.Instance));
        }
        var instances = endpointInstances.FindInstances(route.Endpoint).Select(e => transportAddressTranslation(e)).ToArray();
        var distributionContext = new DistributionContext(instances, outgoingLogicalMessage, messageId, headers, transportAddressTranslation, contextBag);
        var selectedInstanceAddress = distributionPolicy.GetDistributionStrategy(route.Endpoint, DistributionStrategyScope.Send).SelectDestination(distributionContext);
        return new UnicastRoutingStrategy(selectedInstanceAddress);
    }

    public UnicastRouteResolver(Func<EndpointInstance, string> transportAddressTranslation, EndpointInstances endpointInstances, IDistributionPolicy distributionPolicy)
    {
        this.transportAddressTranslation = transportAddressTranslation;
        this.endpointInstances = endpointInstances;
        this.distributionPolicy = distributionPolicy;
    }

    Func<EndpointInstance, string> transportAddressTranslation;
    EndpointInstances endpointInstances;
    IDistributionPolicy distributionPolicy;

}