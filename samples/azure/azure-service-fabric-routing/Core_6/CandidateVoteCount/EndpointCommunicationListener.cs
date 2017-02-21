using System;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using NServiceBus;
using Shared;
using ZipCodeVoteCount;

namespace CandidateVoteCount
{
    using Contracts;

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

            var partitionInfo = await ServicePartitionQueryHelper.QueryServicePartitions(context.ServiceName, context.PartitionId).ConfigureAwait(false);

            var endpointConfiguration = new EndpointConfiguration("CandidateVoteCount");

            var transportConfig = endpointConfiguration.ApplyCommonConfiguration();

            #region ConfigureLocalPartitions-CandidateVoteCount

            endpointConfiguration.MakeInstanceUniquelyAddressable(partitionInfo.LocalPartitionKey);

            transportConfig.Routing().RegisterPartitionsForThisEndpoint(partitionInfo.LocalPartitionKey, partitionInfo.Partitions);

            #endregion

            #region ConfigureReceiverSideDistribution-CandidateVoteCount

            var receiverSideDistributionConfig = transportConfig.Routing().EnableReceiverSideDistribution(partitionInfo.Partitions);
            receiverSideDistributionConfig.AddPartitionMappingForMessageType<VotePlaced>(m => m.Candidate);

            #endregion

            #region ConfigureSenderSideRouting-CandidateVoteCount

            var remotePartitions = new[] {"33000", "66000", "99000"};

            Func<TrackZipCode, string> convertStringZipCodeToHighKey = message =>
            {
                var zipCodeAsNumber = Convert.ToInt32(message.ZipCode);
                // 00000..33000 => 33000 33001..66000 => 66000 66001..99000 => 99000
                if (zipCodeAsNumber >= 0 && zipCodeAsNumber <= 33000)
                {
                    return "33000";
                }

                if (zipCodeAsNumber > 33000 && zipCodeAsNumber <= 66000)
                {
                    return "66000";
                }

                if (zipCodeAsNumber > 66000 && zipCodeAsNumber <= 99000)
                {
                    return "99000";
                }
                throw new Exception($"Invalid zip code '{zipCodeAsNumber}' for message of type '{message.GetType()}'.");
            };

            var senderSideDistributionConfig = transportConfig.Routing().RegisterPartitionedDestinationEndpoint("ZipCodeVoteCount", remotePartitions);

            senderSideDistributionConfig.AddPartitionMappingForMessageType<TrackZipCode>(m => convertStringZipCodeToHighKey(m));

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