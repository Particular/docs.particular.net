using System.Linq;
using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;
using NServiceBus.Routing;

namespace Shared
{
    public static class EndpointPartitioningExtensions
    {
        public static void RegisterPartitionsForThisEndpoint(this EndpointConfiguration endpointConfiguration, string localPartitionKey, string[] allPartitions)
        {
            endpointConfiguration.MakeInstanceUniquelyAddressable(localPartitionKey);

            var settings = endpointConfiguration.GetSettings();

            var endpointName = settings.EndpointName();
            var distributionStrategy = new PartitionAwareDistributionStrategy(endpointName, _ => localPartitionKey, DistributionStrategyScope.Send);

            settings.GetOrCreate<DistributionPolicy>().SetDistributionStrategy(distributionStrategy);

            var destinationEndpointInstances = allPartitions.Select(key => new EndpointInstance(endpointName, key)).ToList();

            settings.GetOrCreate<EndpointInstances>().AddOrReplaceInstances(endpointName, destinationEndpointInstances);

            endpointConfiguration.Pipeline.Register(new HardcodeReplyToAddressToLogicalAddress(settings.InstanceSpecificQueue()), "Hardcodes the ReplyToAddress to the instance specific address of this endpoint.");
        }
    }
}