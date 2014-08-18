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
        Configure.With(b =>
        {
            var ravenPersistence = b.UsePersistence<RavenDB>();
            //todo: 
            c.SetDefaultDocumentStore(documentStore));
        })

        #endregion
    }

}