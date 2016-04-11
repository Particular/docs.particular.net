namespace Raven_2
{
    using NServiceBus;
    using NServiceBus.Persistence;
    using NServiceBus.RavenDB;
    using NServiceBus.RavenDB.Persistence;
    using Raven.Client;
    using Raven.Client.Document;

    class RavenDBConfigure
    {
        void StaleSagas(BusConfiguration busConfiguration)
        {
            #region ravendb-persistence-stale-sagas

            busConfiguration.UsePersistence<RavenDBPersistence>()
                .AllowStaleSagaReads();

            #endregion
        }

        void SharedSessionForSagasAndOutbox(BusConfiguration busConfiguration)
        {
            #region ravendb-persistence-shared-session-for-sagas

            DocumentStore myDocumentStore = new DocumentStore();
            // configure document store properties here

            busConfiguration.UsePersistence<RavenDBPersistence>().UseSharedSession(() =>
            {
                IDocumentSession session = myDocumentStore.OpenSession();
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
            ISessionProvider sessionProvider;

            public MyMessageHandler(ISessionProvider sessionProvider)
            {
                this.sessionProvider = sessionProvider;
            }

            public void Handle(MyMessage message)
            {
                MyDocument doc = new MyDocument();

                sessionProvider.Session.Store(doc);
            }
        }

        #endregion

        void SpecificExternalDocumentStore(BusConfiguration busConfiguration)
        {
            #region ravendb-persistence-specific-external-store

            DocumentStore myDocumentStore = new DocumentStore();
            // configure document store properties here

            busConfiguration.UsePersistence<RavenDBPersistence>()
                .UseDocumentStoreForSubscriptions(myDocumentStore)
                .UseDocumentStoreForSagas(myDocumentStore)
                .UseDocumentStoreForTimeouts(myDocumentStore);

            #endregion
        }

        public void SpecificDocumentStoreViaConnectionString()
        {
            //See the config file
        }

        void ExternalDocumentStore(BusConfiguration busConfiguration)
        {
            #region ravendb-persistence-external-store

            DocumentStore myDocumentStore = new DocumentStore();
            // configure document store properties here

            busConfiguration.UsePersistence<RavenDBPersistence>()
                .SetDefaultDocumentStore(myDocumentStore);

            #endregion
        }

        void ExternalConnectionParameters(BusConfiguration busConfiguration)
        {
            #region ravendb-persistence-external-connection-params

            ConnectionParameters connectionParams = new ConnectionParameters();
            // configure connection params (ApiKey, DatabaseName, Url) here

            busConfiguration.UsePersistence<RavenDBPersistence>()
                .SetDefaultDocumentStore(connectionParams);

            #endregion
        }

        public void SharedDocumentStoreViaConnectionString()
        {
            //See the config file
        }


    }
}