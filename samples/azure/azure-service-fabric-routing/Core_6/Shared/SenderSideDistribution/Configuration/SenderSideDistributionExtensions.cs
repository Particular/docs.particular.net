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

        public static void RegisterPartitionsForThisEndpoint<T>(this RoutingSettings<T> routingSettings, string localPartitionKey, string[] allPartitions) where T : TransportDefinition
        {
            var settings = routingSettings.GetSettings();

            var endpointName = settings.EndpointName();
            var distributionStrategy = new PartitionAwareDistributionStrategy(endpointName, _ => localPartitionKey, DistributionStrategyScope.Send);

            settings.GetOrCreate<DistributionPolicy>().SetDistributionStrategy(distributionStrategy);

            var destinationEndpointInstances = allPartitions.Select(key => new EndpointInstance(endpointName, key)).ToList();

            settings.GetOrCreate<EndpointInstances>().AddOrReplaceInstances(endpointName, destinationEndpointInstances);
        }
    }
}