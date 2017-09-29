using System;
using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;
using NServiceBus.Features;
using NServiceBus.Routing;

#region forwarding-routing-extensions
public static class RoutingExtensions
{
    public static void ForwardToEndpoint(this RoutingSettings routing, Type messageTypeToForward, string destinationEndpointName)
    {
        var settings = routing.GetSettings();

        var endpointRoute = UnicastRoute.CreateFromEndpointName(destinationEndpointName);

        settings.GetOrCreate<ForwardingAddressDirectory>()
            .ForwardToRoute(messageTypeToForward, endpointRoute);

        settings.EnableFeatureByDefault<LeaveForwardingAddressFeature>();
    }
}
#endregion
