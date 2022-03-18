using Microsoft.Extensions.DependencyInjection;
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
        var pipeline = context.Pipeline;
        var forwardingAddressDirectory = settings.Get<ForwardingAddressDirectory>();

        pipeline.Register(sp =>
        {
            var routeResolver = new UnicastRouteResolver(
                sp.GetRequiredService<ITransportAddressResolver>(),
                settings.Get<EndpointInstances>(),
                settings.Get<DistributionPolicy>()
            );

            return new RerouteMessagesWithForwardingAddress(
                forwardingAddressDirectory.ToLookup(),
                routeResolver
            );
        }, "Finds forwarding addresses and resolves them");

        pipeline.Register(new ForwardMessagesWithForwardingAddress(), "Forwards messages to their matching forwarding addresses");
    }
}

#endregion