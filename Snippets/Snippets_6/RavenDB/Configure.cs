namespace Snippets5.RavenDB
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.RavenDB;
    using Raven.Client;
    using Raven.Client.Document;

    class RavenDBConfigure
    {
        public void SharedSessionForSagasAndOutbox()
        {
            #region ravendb-persistence-shared-session-for-sagas

            DocumentStore myDocumentStore = new DocumentStore();
            // configure document store properties here

            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.UsePersistence<RavenDBPersistence>().UseSharedAsyncSession(() =>
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

                return context.GetRavenSession().StoreAsync(doc);
            }
        }

        #endregion

        public void SpecificExternalDocumentStore()
        {
            #region ravendb-persistence-specific-external-store

            DocumentStore myDocumentStore = new DocumentStore();
            // configure document store properties here

            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.UsePersistence<RavenDBPersistence>()
                .UseDocumentStoreForSubscriptions(myDocumentStore)
                .UseDocumentStoreForSagas(myDocumentStore)
                .UseDocumentStoreForTimeouts(myDocumentStore);

            #endregion
        }

        public void SpecificDocumentStoreViaConnectionString()
        {
            //See the config file
        }

        public void ExternalDocumentStore()
        {
            #region ravendb-persistence-external-store

            DocumentStore myDocumentStore = new DocumentStore();
            // configure document store properties here


            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.UsePersistence<RavenDBPersistence>()
                .SetDefaultDocumentStore(myDocumentStore);

            #endregion
        }

        public void ExternalConnectionParameters()
        {
            #region ravendb-persistence-external-connection-params

            ConnectionParameters connectionParams = new ConnectionParameters();
            // configure connection params (ApiKey, DatabaseName, Url) here

            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.UsePersistence<RavenDBPersistence>()
                .SetDefaultDocumentStore(connectionParams);

            #endregion
        }

        public void SharedDocumentStoreViaConnectionString()
        {
            //See the config file
        }


    }
}