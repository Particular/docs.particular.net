using System;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using NServiceBus;

public class EndpointCommunicationListener :
    ICommunicationListener
{
    StatefulServiceContext context;
    IEndpointInstance endpointInstance;

    public EndpointCommunicationListener(StatefulServiceContext context)
    {
        this.context = context;
    }

    public async Task<string> OpenAsync(CancellationToken cancellationToken)
    {
        Logger.Log = m => ServiceEventSource.Current.ServiceMessage(context, m);

        var partitionInfo = await ServicePartitionQueryHelper.QueryServicePartitions(context.ServiceName, context.PartitionId)
            .ConfigureAwait(false);

        var endpointConfiguration = new EndpointConfiguration("CandidateVoteCount");

        var transport = endpointConfiguration.ApplyCommonConfiguration();

        ConfigureLocalPartitionsCandidateVoteCount(endpointConfiguration, partitionInfo);

        ConfigureReceiverSideDistributionCandidateVoteCount(transport, partitionInfo);

        ConfigureSenderSideRoutingCandidateVoteCount(transport);

        endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        return null;
    }

    static void ConfigureLocalPartitionsCandidateVoteCount(EndpointConfiguration endpointConfiguration, PartitionsInformation partitionInfo)
    {
        endpointConfiguration.RegisterPartitionsForThisEndpoint(
            localPartitionKey: partitionInfo.LocalPartitionKey,
            allPartitionKeys: partitionInfo.Partitions);
    }

    static void ConfigureReceiverSideDistributionCandidateVoteCount(TransportExtensions<AzureServiceBusTransport> transportConfig, PartitionsInformation partitionInfo)
    {
        #region ConfigureReceiverSideDistribution-CandidateVoteCount

        var routing = transportConfig.Routing();
        var receiverSideDistribution = routing.EnableReceiverSideDistribution(partitionInfo.Partitions);
        receiverSideDistribution.AddPartitionMappingForMessageType<VotePlaced>(
            mapMessageToPartitionKey: votePlaced =>
            {
                return votePlaced.Candidate;
            });

        #endregion
    }

    static void ConfigureSenderSideRoutingCandidateVoteCount(TransportExtensions<AzureServiceBusTransport> transportConfig)
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

        var routing = transportConfig.Routing();
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