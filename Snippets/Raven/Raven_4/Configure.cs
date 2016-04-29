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
            var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
            persistence.UseSharedAsyncSession(() =>
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
            var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
            persistence.UseDocumentStoreForSubscriptions(myDocumentStore);
            persistence.UseDocumentStoreForSagas(myDocumentStore);
            persistence.UseDocumentStoreForTimeouts(myDocumentStore);
            persistence.UseDocumentStoreForGatewayDeduplication(myDocumentStore);

            #endregion
        }

        void CreateSpecificDocumentStoreByFunc(EndpointConfiguration endpointConfiguration)
        {
            #region ravendb-persistence-specific-create-store-by-func
            
            var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
            persistence.UseDocumentStoreForSubscriptions(readOnlySettings =>
            {
                DocumentStore myDocumentStore = new DocumentStore();
                // configure document store properties here
                return myDocumentStore;
            });
            persistence.UseDocumentStoreForSagas(readOnlySettings =>
            {
                DocumentStore myDocumentStore = new DocumentStore();
                // configure document store properties here
                return myDocumentStore;
            });
            persistence.UseDocumentStoreForTimeouts(readOnlySettings =>
            {
                DocumentStore myDocumentStore = new DocumentStore();
                // configure document store properties here
                return myDocumentStore;
            });
            persistence.UseDocumentStoreForGatewayDeduplication(readOnlySettings =>
            {
                DocumentStore myDocumentStore = new DocumentStore();
                // configure document store properties here
                return myDocumentStore;
            });

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
            var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
            persistence.SetDefaultDocumentStore(myDocumentStore);

            #endregion
        }

        void ExternalConnectionParameters(EndpointConfiguration endpointConfiguration)
        {
            #region ravendb-persistence-external-connection-params

            ConnectionParameters connectionParams = new ConnectionParameters();
            // configure connection params (ApiKey, DatabaseName, Url) here
            var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
            persistence.SetDefaultDocumentStore(connectionParams);

            #endregion
        }

        void CreateDocumentStoreByFunc(EndpointConfiguration endpointConfiguration)
        {
            #region ravendb-persistence-create-store-by-func

            var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
            persistence.SetDefaultDocumentStore(readOnlySettings =>
            {
                DocumentStore myDocumentStore = new DocumentStore();
                // configure document store properties here
                return myDocumentStore;
            });

            #endregion
        }

        void SharedDocumentStoreViaConnectionString()
        {
            //See the config file
        }


    }
}