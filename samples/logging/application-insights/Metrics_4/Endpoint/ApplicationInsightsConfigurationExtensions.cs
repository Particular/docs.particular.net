using Microsoft.ApplicationInsights.Extensibility;
using NServiceBus;
using NServiceBus.Configuration.AdvancedExtensibility;

public static class ApplicationInsightsConfigurationExtensions
{
    public static void EnableApplicationInsights(this EndpointConfiguration configuration, TelemetryConfiguration telemetryConfiguration)
    {
        #region enable-nsb-metrics
        configuration.EnableMetrics();
        #endregion
        configuration.EnableFeature<ApplicationInsightsFeature>();
        configuration.GetSettings().Set(telemetryConfiguration);
    }
}
