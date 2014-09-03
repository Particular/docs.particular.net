using NServiceBus;
using NServiceBus.Persistence;
using Raven.Client.Document;


public class CustomRavenConfig
{
    public void Simple()
    {
        #region CustomRavenConfigV5

        var documentStore = new DocumentStore
        {
            Url = "http://localhost:8080",
            DefaultDatabase = "MyDatabase",
        };
        
        documentStore.Initialize();

        var configuration = new BusConfiguration();

        configuration.UsePersistence<RavenDBPersistence>()
            .SetDefaultDocumentStore(documentStore);
        #endregion
    }

}