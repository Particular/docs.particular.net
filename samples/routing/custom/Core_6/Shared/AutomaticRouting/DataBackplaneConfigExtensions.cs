using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;
using NServiceBus.Features;
using NServiceBus.Persistence;

public static class AutomaticRoutingConfigExtensions
{
    public static void EnableAutomaticRouting(this EndpointConfiguration endpointConfiguration, string connectionString)
    {
        endpointConfiguration.GetSettings().Set("NServiceBus.AutomaticRouting.ConnectionString", connectionString);
        endpointConfiguration.EnableFeature<AutomaticRoutingFeature>();
    }
}
