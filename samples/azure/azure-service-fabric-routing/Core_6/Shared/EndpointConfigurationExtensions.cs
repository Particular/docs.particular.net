using System;
using System.Linq;
using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;
using NServiceBus.Routing;

namespace Shared
{
    public static class EndpointConfigurationExtensions
    {
        public static TransportExtensions<AzureServiceBusTransport> ApplyCommonConfiguration(this EndpointConfiguration endpointConfiguration)
        {
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");
            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.Recoverability().DisableLegacyRetriesSatellite();

            var transportConfig = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
            var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception("Could not read the 'AzureServiceBus.ConnectionString' environment variable. Check the sample prerequisites.");
            }
            transportConfig.ConnectionString(connectionString);
            transportConfig.UseForwardingTopology();

            return transportConfig;
        }

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
