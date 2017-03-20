using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using NServiceBus;
using NServiceBus.Persistence.ServiceFabric;

class GettingStarted
{
    void GettingStartedUsage(IReliableStateManager statemanager, EndpointConfiguration endpointConfiguration)
    {
        #region ServiceFabricPersistenceConfiguration
        var persistence = endpointConfiguration.UsePersistence<ServiceFabricPersistence>();
        persistence.StateManager(statemanager);
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

    class CustomTransactionHandler : IHandleMessages<Message>
    {
        #region CustomTransaction
        public async Task Handle(Message message, IMessageHandlerContext context)
        {
            var session = context.SynchronizedStorageSession.ServiceFabricSession();
            var stateManager = session.StateManager;
            using (var transaction = stateManager.CreateTransaction())
            {
                var dictionary = await stateManager.GetOrAddAsync<IReliableDictionary<string, string>>(transaction, "specialCollection");
                await dictionary.AddOrUpdateAsync(transaction, "key", _ => "value", (_, __) => "value");
                await transaction.CommitAsync();
            }
        }
        #endregion
    }
}