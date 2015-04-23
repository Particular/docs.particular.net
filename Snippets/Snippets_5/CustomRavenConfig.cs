using NServiceBus;
using NServiceBus.Persistence;
using Raven.Client.Document;


public class CustomRavenConfig
{
    public void Simple()
    {
        #region CustomRavenConfig

        DocumentStore documentStore = new DocumentStore
        {
            Url = "http://localhost:8080",
            DefaultDatabase = "MyDatabase",
        };
        
        documentStore.Initialize();

        BusConfiguration busConfiguration = new BusConfiguration();

        busConfiguration.UsePersistence<RavenDBPersistence>()
            .SetDefaultDocumentStore(documentStore);
        #endregion
    }

}