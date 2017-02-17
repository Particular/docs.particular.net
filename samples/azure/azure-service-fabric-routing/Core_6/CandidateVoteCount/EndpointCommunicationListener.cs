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
            transportConfig.Routing().RouteToEndpoint(typeof(TrackZipCode), "ZipCodeVoteCount");

            Func<object, string> mapper = message =>
            {
                var trackZipCode = message as TrackZipCode;
                if (trackZipCode != null)
                {
                    var zipCodeAsNumber = Convert.ToInt32(trackZipCode.ZipCode);
                    // 00000..33000 => 33000 34000..66000 => 66000 67000..99000 => 99000
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
                }

                throw new Exception($"No partition mapping is found for message type '{message.GetType()}'.");
            };

            var internalSettings = endpointConfiguration.GetSettings();
            var policy = internalSettings.GetOrCreate<DistributionPolicy>();
            var instances = internalSettings.GetOrCreate<EndpointInstances>();

            policy.SetDistributionStrategy(new PartitionAwareDistributionStrategy("ZipCodeVoteCount", mapper, DistributionStrategyScope.Send, partitionedEndpointConfiguration.LocalPartitionKey));

            using (var client = new FabricClient())
            {
                var servicePartitionList = await client.QueryManager.GetPartitionListAsync(new Uri("fabric:/ServiceFabricRouting/ZipCodeVoteCount")).ConfigureAwait(false);
                var partitions = new List<EndpointInstance>();
                foreach (var partition in servicePartitionList)
                {
                    partitions.Add(new EndpointInstance("ZipCodeVoteCount", partition.PartitionInformation.Kind == ServicePartitionKind.Int64Range ? ((Int64RangePartitionInformation)partition.PartitionInformation).HighKey.ToString() : ((NamedPartitionInformation)partition.PartitionInformation).Name));
                }

                instances.AddOrReplaceInstances("ZipCodeVoteCount", partitions);
            }

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