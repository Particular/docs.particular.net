using NServiceBus;
using NServiceBus.Configuration.AdvancedExtensibility;

public static class AutomaticRoutingConfigExtensions
{
    public static AdvertisePublishingSettings EnableAutomaticRouting(this EndpointConfiguration endpointConfiguration, string connectionString)
    {
        var settings = endpointConfiguration.GetSettings();
        settings.Set("NServiceBus.AutomaticRouting.ConnectionString", connectionString);
        endpointConfiguration.EnableFeature<AutomaticRoutingFeature>();

        return new AdvertisePublishingSettings(settings);
    }
}