using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;

#region FeatureToggleConfigurationExtensions
public static class FeatureToggleConfigurationExtensions
{
    public static FeatureToggleSettings EnableFeatureToggles(this EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.EnableFeature<FeatureToggles>();
        return endpointConfiguration.GetSettings().GetOrCreate<FeatureToggleSettings>();
    }
}
#endregion