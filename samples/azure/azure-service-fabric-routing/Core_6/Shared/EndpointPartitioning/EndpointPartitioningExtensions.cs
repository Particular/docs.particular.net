using System.Linq;
using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;
using NServiceBus.Routing;

public static class EndpointPartitioningExtensions
{
    public static void RegisterPartitionsForThisEndpoint(this EndpointConfiguration endpointConfiguration, string localPartitionKey, string[] allPartitionKeys)
    {
        endpointConfiguration.MakeInstanceUniquelyAddressable(localPartitionKey);

        var settings = endpointConfiguration.GetSettings();

        var endpointName = settings.EndpointName();
        var distributionStrategy = new PartitionAwareDistributionStrategy(endpointName, _ => localPartitionKey, DistributionStrategyScope.Send);

        var distributionPolicy = settings.GetOrCreate<DistributionPolicy>();
        distributionPolicy.SetDistributionStrategy(distributionStrategy);

        var destinationEndpointInstances = allPartitionKeys.Select(key => new EndpointInstance(endpointName, key)).ToList();

        var endpointInstances = settings.GetOrCreate<EndpointInstances>();
        endpointInstances.AddOrReplaceInstances(endpointName, destinationEndpointInstances);

        var pipeline = endpointConfiguration.Pipeline;
        var addressToLogicalAddress = new HardcodeReplyToAddressToLogicalAddress(settings.InstanceSpecificQueue());
        pipeline.Register(addressToLogicalAddress, "Hardcodes the ReplyToAddress to the instance specific address of this endpoint.");
    }
}