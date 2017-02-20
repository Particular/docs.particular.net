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
            var distributionConfiguration = new PartitionAwareSenderSideDistributionConfiguration(settings, routingSettings, destinationEndpoint, partitions);
            var policy = settings.GetOrCreate<DistributionPolicy>();

            policy.SetDistributionStrategy(new PartitionAwareDistributionStrategy(destinationEndpoint, distributionConfiguration.MapMessageToPartitionKey, DistributionStrategyScope.Send));

            var destinationEndpointInstances = partitions.Select(key => new EndpointInstance(destinationEndpoint, key)).ToList();

            var instances = settings.GetOrCreate<EndpointInstances>();
            instances.AddOrReplaceInstances(destinationEndpoint, destinationEndpointInstances);

            return distributionConfiguration;
        }
    }
}