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

        endpointConfiguration = new EndpointConfiguration("ZipCodeVoteCount");

        endpointConfiguration.ApplyCommonConfiguration(stateManager);

        #region ApplyPartitionConfigurationToEndpoint-ZipCodeVoteCount

        endpointConfiguration.RegisterPartitionsForThisEndpoint(
            localPartitionKey: partitionInfo.LocalPartitionKey,
            allPartitionKeys: partitionInfo.Partitions);

        #endregion

        return null;
    }

    public async Task Run()
    {
        if (endpointConfiguration == null)
        {
            var message = $"{nameof(EndpointCommunicationListener)} Run() method should be invoked after communication listener has been opened and not before.";

            Logger.Log(message);
            throw new Exception(message);
        }

        var zipcodeVotes = await stateManager.TryGetAsync<IReliableDictionary<Guid, SagaEntry>>("zipcode-votes")
            .ConfigureAwait(false);
        if (zipcodeVotes.HasValue)
        {
            await zipcodeVotes.Value.ClearAsync()
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