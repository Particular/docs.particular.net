using Microsoft.ApplicationInsights.Extensibility;
using NServiceBus;
using NServiceBus.Configuration.AdvancedExtensibility;

public static class ApplicationInsightsConfigurationExtensions
{
    public static void EnableApplicationInsights(this EndpointConfiguration configuration, TelemetryConfiguration telemetryConfiguration)
    {
        configuration.EnableFeature<ApplicationInsightsFeature>();
        configuration.GetSettings().Set(telemetryConfiguration);
    }
}
