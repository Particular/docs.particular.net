using NServiceBus;
using NServiceBus.Persistence;
using Raven.Client.Document;


public class CustomRavenConfig
{
    public void Simple()
    {
        #region CustomRavenConfigV5

        using (var documentStore = new DocumentStore
        {
            Url = "http://localhost:8080",
            DefaultDatabase = "MyDatabase",
        })
            documentStore.Initialize();


        var configuration = new BusConfiguration();

        configuration.UsePersistence<RavenDB>();
            //todo: 
            //.SetDefaultDocumentStore(documentStore));
        #endregion
    }

}