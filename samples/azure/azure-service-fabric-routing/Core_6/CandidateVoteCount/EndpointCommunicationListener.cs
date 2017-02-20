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

            //Determine which partition this endpoint is handling
            string localPartitionKey;
            IEnumerable<EndpointInstance> endpointInstances;
            using (var client = new FabricClient())
            {
                var servicePartitionList = await client.QueryManager.GetPartitionListAsync(context.ServiceName).ConfigureAwait(false);
                var servicePartitions =
                    servicePartitionList.Select(x => x.PartitionInformation).Cast<NamedPartitionInformation>().ToList();

                endpointInstances = servicePartitions.Select(x => new EndpointInstance("CandidateVoteCount", x.Name));

                #region ReceiverSideRoutingCandidateVoteCount

                var discriminators = new HashSet<string>(servicePartitions.Select(x => x.Name));

                Func<object, string> candidateMapper = message =>
                {
                    var votePlaced = message as VotePlaced;
                    if (votePlaced != null)
                    {
                        return votePlaced.Candidate;
                    }

                    throw new Exception($"No partition mapping is found for message type '{message.GetType()}'.");
                };

                endpointConfiguration.EnableReceiverSideDistribution(discriminators, candidateMapper, m => ServiceEventSource.Current.ServiceMessage(context, m));

                #endregion

                localPartitionKey = servicePartitions.Single(p => p.Id == context.PartitionId).Name;
            }

            // Set the endpoint instance discriminator using the partition key
            endpointConfiguration.MakeInstanceUniquelyAddressable(localPartitionKey);

            // Register the Service context for later use
            endpointConfiguration.RegisterComponents(components => components.RegisterSingleton(context));

            #region ConfigureSenderSideDistributionCandidateVoteCount

            var internalSettings = endpointConfiguration.GetSettings();
            var policy = internalSettings.GetOrCreate<DistributionPolicy>();
            var instances = internalSettings.GetOrCreate<EndpointInstances>();

            policy.SetDistributionStrategy(new PartitionAwareDistributionStrategy("CandidateVoteCount", message => localPartitionKey, DistributionStrategyScope.Send));
            instances.AddOrReplaceInstances("CandidateVoteCount", endpointInstances.ToList());

            #endregion

            #region SenderSideRoutingZipCodeVoteCount

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

            var distributionConfig = transportConfig.Routing().RegisterPartitionedDestinationEndpoint("ZipCodeVoteCount", new[] { "33000", "66000", "99000" });
            distributionConfig.AddPartitionMappingForMessageType<TrackZipCode>(message => convertStringZipCodeToHighKey(message.GetType(), message.ZipCode));

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