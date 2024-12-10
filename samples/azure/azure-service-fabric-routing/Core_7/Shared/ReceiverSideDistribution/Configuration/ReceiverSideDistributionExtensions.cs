using NServiceBus;
using NServiceBus.Configuration.AdvancedExtensibility;
using NServiceBus.Features;

public static class ReceiverSideDistributionExtensions
{
    public static PartitionAwareReceiverSideDistributionConfiguration EnableReceiverSideDistribution(this RoutingSettings routingSettings, string[] discriminators)
    {
        var settings = routingSettings.GetSettings();

        if (!settings.TryGet(out PartitionAwareReceiverSideDistributionConfiguration config))
        {
            config = new PartitionAwareReceiverSideDistributionConfiguration(routingSettings, discriminators);
            settings.Set(config);
            settings.Set(typeof(ReceiverSideDistribution).FullName, FeatureState.Enabled);
        }

        return config;
    }
}