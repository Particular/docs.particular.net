using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;
using NServiceBus.Features;
using NServiceBus.Persistence;

public static class AutomaticRoutingConfigExtensions
{
    public static AdvertisePublishingSettings EnableAutomaticRouting(this EndpointConfiguration endpointConfiguration, string connectionString)
    {
        endpointConfiguration.GetSettings().Set("NServiceBus.AutomaticRouting.ConnectionString", connectionString);
        endpointConfiguration.EnableFeature<AutomaticRoutingFeature>();

        return new AdvertisePublishingSettings(endpointConfiguration.GetSettings());
    }
}