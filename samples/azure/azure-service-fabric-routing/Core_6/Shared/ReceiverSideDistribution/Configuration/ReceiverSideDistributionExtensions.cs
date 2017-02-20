using System;
using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;
using NServiceBus.Features;

namespace Shared
{
    public static class ReceiverSideDistributionExtensions
    {
        public static PartitionAwareReceiverSideDistributionConfiguration EnableReceiverSideDistribution(this RoutingSettings routingSettings, string[] discriminators, Action<string> logger = null)
        {
            var settings = routingSettings.GetSettings();

            PartitionAwareReceiverSideDistributionConfiguration config;
            if (!settings.TryGet(out config))
            {
                config = new PartitionAwareReceiverSideDistributionConfiguration(routingSettings, discriminators, logger);
                settings.Set<PartitionAwareReceiverSideDistributionConfiguration>(config);
                settings.Set(typeof(ReceiverSideDistribution).FullName, FeatureState.Enabled);
            }

            return config;
        }
    }
}