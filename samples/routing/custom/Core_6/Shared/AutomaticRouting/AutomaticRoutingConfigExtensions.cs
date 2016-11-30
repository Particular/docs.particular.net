using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;

public static class AutomaticRoutingConfigExtensions
{
    public static void EnableAutomaticRouting(this EndpointConfiguration endpointConfiguration, string connectionString)
    {
        var settings = endpointConfiguration.GetSettings();
        settings.Set("NServiceBus.AutomaticRouting.ConnectionString", connectionString);
        endpointConfiguration.EnableFeature<AutomaticRoutingFeature>();
    }
}