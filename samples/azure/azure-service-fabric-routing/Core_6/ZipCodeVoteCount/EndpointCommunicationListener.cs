using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contracts;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;
using NServiceBus.Routing;

namespace ZipCodeVoteCount
{
    public class EndpointCommunicationListener : ICommunicationListener
    {
        private StatefulServiceContext context;
        private IEndpointInstance endpointInstance;

        public EndpointCommunicationListener(StatefulServiceContext context)
        {
            this.context = context;
        }

        public async Task<string> OpenAsync(CancellationToken cancellationToken)
        {
            Int64RangePartitionInformation rangePartitionInformation;
            using (var client = new FabricClient())
            {
                var servicePartitionList = await client.QueryManager.GetPartitionListAsync(context.ServiceName, context.PartitionId).ConfigureAwait(false);
                rangePartitionInformation = servicePartitionList.Select(x => x.PartitionInformation).Cast<Int64RangePartitionInformation>().Single(p => p.Id == context.PartitionId);
            }

            var endpointConfiguration = new EndpointConfiguration("ZipCodeVoteCount");
            endpointConfiguration.MakeInstanceUniquelyAddressable(rangePartitionInformation.HighKey.ToString());

            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");
            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.Recoverability().DisableLegacyRetriesSatellite();
            endpointConfiguration.RegisterComponents(components => components.RegisterSingleton(context));

            var internalSettings = endpointConfiguration.GetSettings();

            var policy = internalSettings.GetOrCreate<DistributionPolicy>();
            policy.SetDistributionStrategy(new LocalPartitionAwareDistributionStrategy(rangePartitionInformation.HighKey.ToString(), "ZipCodeVoteCount", DistributionStrategyScope.Send));

            using (var client = new FabricClient())
            {
                var servicePartitionList = await client.QueryManager.GetPartitionListAsync(context.ServiceName).ConfigureAwait(false);
                var partitions = new List<EndpointInstance>();
                foreach (var partition in servicePartitionList)
                {
                    partitions.Add(new EndpointInstance("ZipCodeVoteCount", partition.PartitionInformation.Kind == ServicePartitionKind.Int64Range ? ((Int64RangePartitionInformation)partition.PartitionInformation).HighKey.ToString() : ((NamedPartitionInformation)partition.PartitionInformation).Name));
                }

                var instances = internalSettings.GetOrCreate<EndpointInstances>();
                instances.AddOrReplaceInstances("ZipCodeVoteCount", partitions);
            }

            var transportConfig = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
            var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception("Could not read the 'AzureServiceBus.ConnectionString' environment variable. Check the sample prerequisites.");
            }
            transportConfig.ConnectionString(connectionString);
            transportConfig.UseForwardingTopology();

            endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);
            return null;
        }

        public Task CloseAsync(CancellationToken cancellationToken)
        {
            return endpointInstance.Stop();
        }

        public void Abort()
        {
            // Fire & Forget Close
            CloseAsync(CancellationToken.None);
        }
    }
}