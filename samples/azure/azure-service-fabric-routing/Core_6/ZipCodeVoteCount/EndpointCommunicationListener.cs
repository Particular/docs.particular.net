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
            var endpointConfiguration = new EndpointConfiguration("ZipCodeVoteCount");

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

            endpointConfiguration.RegisterComponents(components => components.RegisterSingleton(context));

            string localPartitionKey;
            IEnumerable<EndpointInstance> instanceList;
            using (var client = new FabricClient())
            {
                var servicePartitionList = await client.QueryManager.GetPartitionListAsync(context.ServiceName).ConfigureAwait(false);
                var partitionInformations = servicePartitionList.Select(x => x.PartitionInformation).Cast<Int64RangePartitionInformation>().ToList();
                instanceList = partitionInformations.Select(p => new EndpointInstance("ZipCodeVoteCount", p.HighKey.ToString()));
                localPartitionKey = partitionInformations.Single(p => p.Id == context.PartitionId).HighKey.ToString();
            }

            endpointConfiguration.MakeInstanceUniquelyAddressable(localPartitionKey);

            var internalSettings = endpointConfiguration.GetSettings();
            var policy = internalSettings.GetOrCreate<DistributionPolicy>();
            var instances = internalSettings.GetOrCreate<EndpointInstances>();

            policy.SetDistributionStrategy(new LocalPartitionAwareDistributionStrategy(localPartitionKey, "ZipCodeVoteCount", DistributionStrategyScope.Send));
            instances.AddOrReplaceInstances("ZipCodeVoteCount", instanceList.ToList());

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