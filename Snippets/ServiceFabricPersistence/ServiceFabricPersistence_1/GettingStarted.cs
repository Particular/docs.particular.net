using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;
using NServiceBus;
using NServiceBus.Persistence.ServiceFabric;

class GettingStarted
{
    async Task GettingStartedUsage()
    {
        #region ServiceFabricPersistenceConfiguration

        var endpointConfiguration = new EndpointConfiguration("myendpoint");
        endpointConfiguration.UsePersistence<ServiceFabricPersistence>();

        var startableEndpoint = await Endpoint.Create(endpointConfiguration)
            .ConfigureAwait(false);
        var endpointInstance = await startableEndpoint.Start()
            .ConfigureAwait(false);

        #endregion
    }

    class Message : IMessage { }

    #region ServiceFabricPersistenceSynchronizedSession
    class Handler : IHandleMessages<Message>
    {
        public async Task Handle(Message message, IMessageHandlerContext context)
        {
            var session = context.SynchronizedStorageSession.ServiceFabricSession();
            var stateManager = session.StateManager;
            var transaction = session.Transaction;

            var dictionary = await stateManager.GetOrAddAsync<IReliableDictionary<string, string>>(transaction, "state");

            await dictionary.AddOrUpdateAsync(transaction, "key", _ => "value", (_, __) => "value");
        }
    }
    #endregion
}