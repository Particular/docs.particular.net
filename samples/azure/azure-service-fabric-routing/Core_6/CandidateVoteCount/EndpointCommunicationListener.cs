using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;
using NServiceBus.Routing;
using PartionAwareSenderSideDistribution;
using PartitionedEndpointConfig;
using Shared;

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
            var endpointConfiguration = new EndpointConfiguration("CandidateVoteCount");

            var transportConfig = endpointConfiguration.ApplyCommonConfiguration();

            var partitionedEndpointConfiguration = await endpointConfiguration.ConfigureNamedPartitionedEndpoint(context);

            #region Configure Receiver-Side routing for CandidateVoteCount

            var receiverSideDistributionConfig = endpointConfiguration.EnableReceiverSideDistribution(partitionedEndpointConfiguration.KnownPartitionKeys, m => ServiceEventSource.Current.ServiceMessage(context, m));

            receiverSideDistributionConfig.AddMappingForMessageType<VotePlaced>(message => message.Candidate);

            #endregion


            #region Configure Sender-Side routing for ZipCodeVoteCount

            var zipCodeVoteCountSenderSideDistributionConfig = transportConfig.RegisterSenderSideDistributionForPartitionedEndpoint("ZipCodeVoteCount", new[] {"33000", "66000", "99000"});

            Func<Type, string, string> convertStringZipCodeToHighKey = (messageType, zipCode) =>
            {
                var zipCodeAsNumber = Convert.ToInt32(zipCode);
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
                throw new Exception($"Invalid zip code '{zipCodeAsNumber}' for message of type '{messageType}'.");
            };

            zipCodeVoteCountSenderSideDistributionConfig.AddMappingForMessageType<TrackZipCode>(message => convertStringZipCodeToHighKey(message.GetType(), message.ZipCode));

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