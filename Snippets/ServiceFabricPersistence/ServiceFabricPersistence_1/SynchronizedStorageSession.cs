namespace ServiceFabricPersistence_1
{
    using System.Threading.Tasks;
    using Microsoft.ServiceFabric.Data.Collections;
    using NServiceBus;
    using NServiceBus.Persistence.ServiceFabric;

    public class SynchronizedStorageSession
    {
        public class Message : IMessage { }

        #region ServiceFabricPersistenceSynchronizedSession-Handler
        public class HandlerThatUsesSession : IHandleMessages<Message>
        {
            public async Task Handle(Message message, IMessageHandlerContext context)
            {
                var session = context.SynchronizedStorageSession.ServiceFabricSession();
                var stateManager = session.StateManager;
                var transaction = session.Transaction;
                var dictionary = await stateManager.GetOrAddAsync<IReliableDictionary<string, string>>(transaction, "state")
                    .ConfigureAwait(false);
                await dictionary.AddOrUpdateAsync(transaction, "key", _ => "value", (_, __) => "value")
                    .ConfigureAwait(false);
            }
        }
        #endregion

        #region ServiceFabricPersistenceSynchronizedSession-Saga

        public class SagaThatUsesSession : Saga<SagaThatUsesSession.SagaData>,
            IHandleMessages<Message>
        {
            public async Task Handle(Message message, IMessageHandlerContext context)
            {
                var session = context.SynchronizedStorageSession.ServiceFabricSession();
                var stateManager = session.StateManager;
                var transaction = session.Transaction;
                var dictionary = await stateManager.GetOrAddAsync<IReliableDictionary<string, string>>(transaction, "state")
                    .ConfigureAwait(false);
                await dictionary.AddOrUpdateAsync(transaction, "key", _ => "value", (_, __) => "value")
                    .ConfigureAwait(false);
            }

            #endregion

            public class SagaData : ContainSagaData
            {
            }

            protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
            {
            }
        }

        class CustomTransactionHandler : IHandleMessages<Message>
        {
            #region CustomTransaction
            public async Task Handle(Message message, IMessageHandlerContext context)
            {
                var session = context.SynchronizedStorageSession.ServiceFabricSession();
                var stateManager = session.StateManager;
                using (var transaction = stateManager.CreateTransaction())
                {
                    var dictionary = await stateManager.GetOrAddAsync<IReliableDictionary<string, string>>(transaction, "specialCollection")
                        .ConfigureAwait(false);
                    await dictionary.AddOrUpdateAsync(transaction, "key", _ => "value", (_, __) => "value")
                        .ConfigureAwait(false);
                    await transaction.CommitAsync()
                        .ConfigureAwait(false);
                }
            }
            #endregion
        }
    }
}