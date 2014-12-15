using System;
using NServiceBus;
using NServiceBus.Persistence;
using Raven.Client.Document;

class ChangeResourceManagerID
{
    public ChangeResourceManagerID()
    {
        #region ChangeResourceManagerID

        const string id = "d5723e19-92ad-4531-adad-8611e6e05c8a";
        var store = new DocumentStore
                    {
                        ResourceManagerId = new Guid(id)
                    };
        store.Initialize();

        var configuration = new BusConfiguration();
        configuration.UsePersistence<RavenDBPersistence>()
            .SetDefaultDocumentStore(store);

        #endregion
    }
}