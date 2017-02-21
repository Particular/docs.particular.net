using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using NServiceBus;
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

            var partitionInfo = await ServicePartitionQueryHelper.QueryServicePartitions(context.ServiceName, context.PartitionId);

            var endpointConfiguration = new EndpointConfiguration("ZipCodeVoteCount");

            endpointConfiguration.ApplyCommonConfiguration();

            #region ConfigureLocalPartitions-ZipCodeVoteCount

            endpointConfiguration.RegisterPartitionsForThisEndpoint(partitionInfo.LocalPartitionKey, partitionInfo.Partitions);

            #endregion

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