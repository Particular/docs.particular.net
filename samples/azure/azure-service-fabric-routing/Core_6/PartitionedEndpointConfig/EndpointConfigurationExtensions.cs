namespace PartitionedEndpointConfig
{
    using System;
    using System.Collections.Generic;
    using NServiceBus;
    using NServiceBus.Configuration.AdvanceExtensibility;
    using System.Fabric;
    using System.Linq;
    using System.Threading.Tasks;
    using NServiceBus.Routing;

    public static class EndpointConfigurationExtensions
    {
        public static ReceiverSideDistributionConfiguration EnableReceiverSideDistribution(this EndpointConfiguration endpointConfiguration, HashSet<string> discriminators, Action<string> logger = null, bool trustReplies = false)
        {
            var receiverSideDistributionConfiguration = new ReceiverSideDistributionConfiguration(discriminators, logger, trustReplies);

            endpointConfiguration.GetSettings().Set<ReceiverSideDistributionConfiguration>(receiverSideDistributionConfiguration);

            endpointConfiguration.EnableFeature<ReceiverSideDistribution>();

            return receiverSideDistributionConfiguration;
        }

        public static Task<PartitionedEndpointInstanceConfiguration> ConfigureInt64RangedPartitionedEndpoint(this EndpointConfiguration endpointConfiguration, StatefulServiceContext context)
        {
            return ConfigurePartitionedEndpoint<Int64RangePartitionInformation>(endpointConfiguration, context, i => i.HighKey.ToString());
        }

        public static Task<PartitionedEndpointInstanceConfiguration> ConfigureNamedPartitionedEndpoint(this EndpointConfiguration endpointConfiguration, StatefulServiceContext context)
        {
            return ConfigurePartitionedEndpoint<NamedPartitionInformation>(endpointConfiguration, context, i => i.Name);
        }

        private static async Task<PartitionedEndpointInstanceConfiguration> ConfigurePartitionedEndpoint<T>(this EndpointConfiguration endpointConfiguration, StatefulServiceContext context, Func<T, string> selectDescriminater) where T: ServicePartitionInformation
        {
            var partitionedEndpointInstanceConfiguration = new PartitionedEndpointInstanceConfiguration();

            using (var client = new FabricClient())
            {
                var servicePartitionList = await client.QueryManager.GetPartitionListAsync(context.ServiceName).ConfigureAwait(false);
                var servicePartitions = servicePartitionList.Select(x => x.PartitionInformation).Cast<T>().ToList();

                var endpointName = endpointConfiguration.GetSettings().EndpointName();

                partitionedEndpointInstanceConfiguration.KnownPartitionKeys = new HashSet<string>(servicePartitions.Select(selectDescriminater));

                var endpointInstances = partitionedEndpointInstanceConfiguration.KnownPartitionKeys.Select(key => new EndpointInstance(endpointName, key));

                partitionedEndpointInstanceConfiguration.LocalPartitionKey = selectDescriminater(servicePartitions.Single(p => p.Id == context.PartitionId));

                // Set the endpoint instance discriminator using the partition key
                endpointConfiguration.MakeInstanceUniquelyAddressable(partitionedEndpointInstanceConfiguration.LocalPartitionKey);

                // Register the Service context for later use
                endpointConfiguration.RegisterComponents(components => components.RegisterSingleton(context));

                #region Configure Local send to own individualized queue distribution strategy

                var internalSettings = endpointConfiguration.GetSettings();
                var policy = internalSettings.GetOrCreate<DistributionPolicy>();
                var instances = internalSettings.GetOrCreate<EndpointInstances>();

                policy.SetDistributionStrategy(new LocalPartitionAwareDistributionStrategy(endpointName, partitionedEndpointInstanceConfiguration.LocalPartitionKey));

                instances.AddOrReplaceInstances(endpointName, endpointInstances.ToList());

                #endregion

                return partitionedEndpointInstanceConfiguration;
            }
        }
    }
}