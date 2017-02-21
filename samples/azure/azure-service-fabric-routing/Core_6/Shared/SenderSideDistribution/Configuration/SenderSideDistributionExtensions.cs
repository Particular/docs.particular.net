using System.Linq;
using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;
using NServiceBus.Routing;
using NServiceBus.Transport;

namespace Shared
{
    public static class SenderSideDistributionExtensions
    {
        public static PartitionAwareSenderSideDistributionConfiguration RegisterPartitionedDestinationEndpoint<T>(this RoutingSettings<T> routingSettings, string destinationEndpoint, string[] partitions) where T : TransportDefinition
        {
            var settings = routingSettings.GetSettings();

            var distributionConfiguration = new PartitionAwareSenderSideDistributionConfiguration(routingSettings, destinationEndpoint, partitions);
            var distributionStrategy = new PartitionAwareDistributionStrategy(destinationEndpoint, distributionConfiguration.MapMessageToPartitionKey, DistributionStrategyScope.Send);

            settings.GetOrCreate<DistributionPolicy>().SetDistributionStrategy(distributionStrategy);

            var destinationEndpointInstances = partitions.Select(key => new EndpointInstance(destinationEndpoint, key)).ToList();

            settings.GetOrCreate<EndpointInstances>().AddOrReplaceInstances(destinationEndpoint, destinationEndpointInstances);

            return distributionConfiguration;
        }
    }
}