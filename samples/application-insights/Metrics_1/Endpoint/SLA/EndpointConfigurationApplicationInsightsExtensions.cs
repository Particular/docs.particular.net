using NServiceBus;

/// <summary>
/// Exposes windows performance counters configuration on <see cref="EndpointConfiguration"/>.
/// </summary>
public static class EndpointConfigurationApplicationInsightsExtensions
{
    /// <summary>
    /// Enables receive statistics and critical time performance counters.
    /// </summary>
    /// <param name="endpointConfiguration">The <see cref="EndpointConfiguration" /> instance to apply the settings to.</param>
    public static ApplicationInsightsSettings EnableApplicationInsights(this EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.EnableFeature<ApplicationInsightsFeature>();
        return new ApplicationInsightsSettings(endpointConfiguration);
    }
}