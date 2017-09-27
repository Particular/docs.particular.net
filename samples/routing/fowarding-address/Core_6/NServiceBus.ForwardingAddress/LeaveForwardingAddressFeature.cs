using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Routing;
using NServiceBus.Transport;

class LeaveForwardingAddressFeature : Feature
{
    protected override void Setup(FeatureConfigurationContext context)
    {
        var forwardingAddressDirectory = context.Settings.Get<ForwardingAddressDirectory>();
        var transportInfrastructure = context.Settings.Get<TransportInfrastructure>();

        var routeResolver = new UnicastRouteResolver(
            i => transportInfrastructure.ToTransportAddress(LogicalAddress.CreateRemoteAddress(i)),
            context.Settings.Get<EndpointInstances>(),
            context.Settings.Get<DistributionPolicy>()
        );

        var rerouteBehavior = new RerouteMessagesWithForwardingAddress(
            forwardingAddressDirectory.ToLookup(),
            routeResolver
        );
        
        var invokeForwardingPipeline = new ForwardMessagesWithForwardingAddress();

        context.Pipeline.Register(rerouteBehavior, "Finds forwarding addresses and resolves them");
        context.Pipeline.Register(invokeForwardingPipeline, "Forwards messages to their matching forwarding addresses");
    }
}