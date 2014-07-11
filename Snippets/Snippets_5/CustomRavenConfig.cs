using NServiceBus;
using NServiceBus.Persistence;
using Raven.Client.Document;


public class CustomRavenConfig
{
    public void Simple()
    {
        // start code CustomRavenConfigV5
        using (var documentStore = new DocumentStore
        {
            Url = "http://localhost:8080",
            DefaultDatabase = "MyDatabase",   
        })
        {
            documentStore.Initialize();
            Configure.With()
                .UsePersistence<RavenDB>(c => c.SetDefaultDocumentStore(documentStore));
        }
        // end code CustomRavenConfigV5
    }

}