﻿namespace ServiceFabricPersistence_3
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
                var dictionary = await stateManager.GetOrAddAsync<IReliableDictionary<string, string>>(transaction, "state");
                await dictionary.AddOrUpdateAsync(transaction, "key", _ => "value", (_, __) => "value");
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
                var dictionary = await stateManager.GetOrAddAsync<IReliableDictionary<string, string>>(transaction, "state");
                await dictionary.AddOrUpdateAsync(transaction, "key", _ => "value", (_, __) => "value");
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
                    var dictionary = await stateManager.GetOrAddAsync<IReliableDictionary<string, string>>(transaction, "specialCollection");
                    await dictionary.AddOrUpdateAsync(transaction, "key", _ => "value", (_, __) => "value");
                    await transaction.CommitAsync();
                }
            }
            #endregion
        }
    }
}