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

            #region Common Endpoint Configuration
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
            #endregion

            //Determine which partition this endpoint is handling
            string localPartitionKey;
            using (var client = new FabricClient())
            {
                var servicePartitionList = await client.QueryManager.GetPartitionListAsync(context.ServiceName).ConfigureAwait(false);
                var servicePartitions =
                    servicePartitionList.Select(x => x.PartitionInformation).Cast<NamedPartitionInformation>().ToList();

                #region Configure Receiver-Side routing for CandidateVoteCount
                var discriminators = new HashSet<string>(servicePartitions.Select(x => x.Name));
                endpointConfiguration.EnableReceiverSideDistribution(
                    discriminators,
                    message => (message as PlaceVote)?.Candidate,
                    m => ServiceEventSource.Current.ServiceMessage(context, m)); //There are no message types that need to be mapped, so this receiver side mapper just returns null for now
                #endregion

                localPartitionKey = servicePartitions.Single(p => p.Id == context.PartitionId).Name;
            }

            // Set the endpoint instance discriminator using the partition key
            endpointConfiguration.MakeInstanceUniquelyAddressable(localPartitionKey);

            // Register the Service context for later use
            endpointConfiguration.RegisterComponents(components => components.RegisterSingleton(context));

            var internalSettings = endpointConfiguration.GetSettings();
            var policy = internalSettings.GetOrCreate<DistributionPolicy>();
            var instances = internalSettings.GetOrCreate<EndpointInstances>();

            #region Configure Local send to own individualized queue distribution strategy
            policy.SetDistributionStrategy(new LocalPartitionAwareDistributionStrategy(localPartitionKey, "CandidateVoteCount", DistributionStrategyScope.Send));

            instances.AddOrReplaceInstances("CandidateVoteCount", new List<EndpointInstance> { new EndpointInstance("CandidateVoteCount", localPartitionKey) });

            #endregion

            #region Configure Sender-Side routing for ZipCodeVoteCount
            transportConfig.Routing().RouteToEndpoint(typeof(TrackZipCode), "ZipCodeVoteCount"); //TODO: Is this really necessary if we are replacing it later?

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

            policy.SetDistributionStrategy(new PartitionAwareDistributionStrategy("ZipCodeVoteCount", mapper, DistributionStrategyScope.Send));

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