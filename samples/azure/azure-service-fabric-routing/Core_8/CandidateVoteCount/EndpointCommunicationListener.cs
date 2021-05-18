using System;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using NServiceBus;
using NServiceBus.Persistence.ServiceFabric;

public class EndpointCommunicationListener :
    ICommunicationListener
{
    StatefulServiceContext context;
    IReliableStateManager stateManager;
    IEndpointInstance endpointInstance;
    EndpointConfiguration endpointConfiguration;

    public EndpointCommunicationListener(StatefulServiceContext context, IReliableStateManager stateManager)
    {
        this.context = context;
        this.stateManager = stateManager;
    }

    public async Task<string> OpenAsync(CancellationToken cancellationToken)
    {
        Logger.Log = m => ServiceEventSource.Current.ServiceMessage(context, m);

        var partitionInfo = await ServicePartitionQueryHelper.QueryServicePartitions(context.ServiceName, context.PartitionId)
            .ConfigureAwait(false);

        endpointConfiguration = new EndpointConfiguration("CandidateVoteCount");

        var transport = endpointConfiguration.ApplyCommonConfiguration(stateManager);

        ConfigureLocalPartitionsCandidateVoteCount(endpointConfiguration, partitionInfo);

        ConfigureReceiverSideDistributionCandidateVoteCount(transport, partitionInfo);

        ConfigureSenderSideRoutingCandidateVoteCount(transport);

        return null;
    }

    static void ConfigureLocalPartitionsCandidateVoteCount(EndpointConfiguration endpointConfiguration, PartitionsInformation partitionInfo)
    {
        endpointConfiguration.RegisterPartitionsForThisEndpoint(
            localPartitionKey: partitionInfo.LocalPartitionKey,
            allPartitionKeys: partitionInfo.Partitions);
    }

    static void ConfigureReceiverSideDistributionCandidateVoteCount(RoutingSettings<AzureServiceBusTransport> routing, PartitionsInformation partitionInfo)
    {
        #region ConfigureReceiverSideDistribution-CandidateVoteCount

        var receiverSideDistribution = routing.EnableReceiverSideDistribution(partitionInfo.Partitions);
        receiverSideDistribution.AddPartitionMappingForMessageType<VotePlaced>(
            mapMessageToPartitionKey: votePlaced =>
            {
                return votePlaced.Candidate;
            });

        #endregion
    }

    static void ConfigureSenderSideRoutingCandidateVoteCount(RoutingSettings<AzureServiceBusTransport> routing)
    {
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

        var senderSideDistribution = routing.RegisterPartitionedDestinationEndpoint(
            destinationEndpoint: "ZipCodeVoteCount",
            partitions: remotePartitions);

        senderSideDistribution.AddPartitionMappingForMessageType<TrackZipCode>(
            mapMessageToPartitionKey: trackZipCode =>
            {
                return convertStringZipCodeToHighKey(trackZipCode);
            });

        #endregion
    }

    public async Task Run()
    {
        if(endpointConfiguration == null)
        {
            var message = $"{nameof(EndpointCommunicationListener)} Run() method should be invoked after communication listener has been opened and not before.";

            Logger.Log(message);
            throw new Exception(message);
        }

        var candidatesVotes = await stateManager.TryGetAsync<IReliableDictionary<Guid, SagaEntry>>("candidate-votes")
            .ConfigureAwait(false);
        if (candidatesVotes.HasValue)
        {
            await candidatesVotes.Value.ClearAsync()
                .ConfigureAwait(false);
        }

        endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
    }

    public Task CloseAsync(CancellationToken cancellationToken)
    {
        return endpointInstance != null ? endpointInstance.Stop() : Task.CompletedTask;
    }

    public void Abort()
    {
        // Fire & Forget Close
        CloseAsync(CancellationToken.None);
    }
}