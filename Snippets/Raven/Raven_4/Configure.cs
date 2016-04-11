namespace Raven_4
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.RavenDB;
    using Raven.Client;
    using Raven.Client.Document;

    class RavenDBConfigure
    {
        void SharedSessionForSagasAndOutbox(EndpointConfiguration endpointConfiguration)
        {
            #region ravendb-persistence-shared-session-for-sagas

            DocumentStore myDocumentStore = new DocumentStore();
            // configure document store properties here
            endpointConfiguration.UsePersistence<RavenDBPersistence>().UseSharedAsyncSession(() =>
            {
                IAsyncDocumentSession session = myDocumentStore.OpenAsyncSession();
                // customize the session properties here
                return session;
            });

            #endregion
        }

        public class MyMessage
        {
        }

        public class MyDocument
        {
        }

        #region ravendb-persistence-shared-session-for-sagas-handler

        public class MyMessageHandler : IHandleMessages<MyMessage>
        {
            public Task Handle(MyMessage message, IMessageHandlerContext context)
            {
                MyDocument doc = new MyDocument();

                IAsyncDocumentSession ravenSession = context.SynchronizedStorageSession.RavenSession();
                return ravenSession.StoreAsync(doc);
            }
        }

        #endregion

        void SpecificExternalDocumentStore(EndpointConfiguration endpointConfiguration)
        {
            #region ravendb-persistence-specific-external-store

            DocumentStore myDocumentStore = new DocumentStore();
            // configure document store properties here
            endpointConfiguration.UsePersistence<RavenDBPersistence>()
                .UseDocumentStoreForSubscriptions(myDocumentStore)
                .UseDocumentStoreForSagas(myDocumentStore)
                .UseDocumentStoreForTimeouts(myDocumentStore);

            #endregion
        }

        void SpecificDocumentStoreViaConnectionString()
        {
            //See the config file
        }

        void ExternalDocumentStore(EndpointConfiguration endpointConfiguration)
        {
            #region ravendb-persistence-external-store

            DocumentStore myDocumentStore = new DocumentStore();
            // configure document store properties here
            endpointConfiguration.UsePersistence<RavenDBPersistence>()
                .SetDefaultDocumentStore(myDocumentStore);

            #endregion
        }

        void ExternalConnectionParameters(EndpointConfiguration endpointConfiguration)
        {
            #region ravendb-persistence-external-connection-params

            ConnectionParameters connectionParams = new ConnectionParameters();
            // configure connection params (ApiKey, DatabaseName, Url) here
            endpointConfiguration.UsePersistence<RavenDBPersistence>()
                .SetDefaultDocumentStore(connectionParams);

            #endregion
        }

        void SharedDocumentStoreViaConnectionString()
        {
            //See the config file
        }


    }
}