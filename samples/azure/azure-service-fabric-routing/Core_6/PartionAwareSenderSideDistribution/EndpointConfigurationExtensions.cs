using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;
using NServiceBus.Routing;
using NServiceBus.Transport;

namespace PartionAwareSenderSideDistribution
{
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class EndpointConfigurationExtensions
    {
        public static PartitionAwareSenderSideDistributionConfiguration<T> RegisterSenderSideDistributionForPartitionedEndpoint<T>(this TransportExtensions<T> transportConfig, string destinationEndpoint, string[] destinationPartitionKeys) where T : TransportDefinition
        {
            var distributionConfiguration = new PartitionAwareSenderSideDistributionConfiguration<T>(destinationEndpoint, transportConfig);

            var internalSettings = transportConfig.GetSettings();

            var policy = internalSettings.GetOrCreate<DistributionPolicy>();

            policy.SetDistributionStrategy(new PartitionAwareDistributionStrategy(destinationEndpoint, distributionConfiguration.MapMessageToPartitionKey, DistributionStrategyScope.Send));

            var destinationEndpointInstances = destinationPartitionKeys.Select(key => new EndpointInstance(destinationEndpoint, key)).ToList();

            var instances = internalSettings.GetOrCreate<EndpointInstances>();
            instances.AddOrReplaceInstances(destinationEndpoint, destinationEndpointInstances);

            return distributionConfiguration;
        }
    }
}
