using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;
using NServiceBus.Routing;
using Shared;

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
            Logger.Log = m => ServiceEventSource.Current.ServiceMessage(context, m);

            var endpointConfiguration = new EndpointConfiguration("ZipCodeVoteCount");

            var transportConfiguration = endpointConfiguration.ApplyCommonConfiguration();

            string localPartitionKey;
            string[] partitions;
            using (var client = new FabricClient())
            {
                var servicePartitionList = await client.QueryManager.GetPartitionListAsync(context.ServiceName).ConfigureAwait(false);
                var partitionInformations = servicePartitionList.Select(x => x.PartitionInformation).Cast<Int64RangePartitionInformation>().ToList();
                partitions = partitionInformations.Select(p => p.HighKey.ToString()).ToArray();
                localPartitionKey = partitionInformations.Single(p => p.Id == context.PartitionId).HighKey.ToString();
            }

            endpointConfiguration.MakeInstanceUniquelyAddressable(localPartitionKey);

            transportConfiguration.Routing().RegisterPartitionsForThisEndpoint(localPartitionKey, partitions);

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