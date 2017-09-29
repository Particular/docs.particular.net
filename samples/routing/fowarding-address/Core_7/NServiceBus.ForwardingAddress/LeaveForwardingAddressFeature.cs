using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Routing;
using NServiceBus.Transport;

#region forwarding-feature

class LeaveForwardingAddressFeature :
    Feature
{
    protected override void Setup(FeatureConfigurationContext context)
    {
        var settings = context.Settings;
        var forwardingAddressDirectory = settings.Get<ForwardingAddressDirectory>();
        var transportInfrastructure = settings.Get<TransportInfrastructure>();

        var routeResolver = new UnicastRouteResolver(
            i => transportInfrastructure.ToTransportAddress(LogicalAddress.CreateRemoteAddress(i)),
            settings.Get<EndpointInstances>(),
            settings.Get<DistributionPolicy>()
        );

        var rerouteBehavior = new RerouteMessagesWithForwardingAddress(
            forwardingAddressDirectory.ToLookup(),
            routeResolver
        );

        var invokeForwardingPipeline = new ForwardMessagesWithForwardingAddress();

        var pipeline = context.Pipeline;
        pipeline.Register(rerouteBehavior, "Finds forwarding addresses and resolves them");
        pipeline.Register(invokeForwardingPipeline, "Forwards messages to their matching forwarding addresses");
    }
}

#endregion