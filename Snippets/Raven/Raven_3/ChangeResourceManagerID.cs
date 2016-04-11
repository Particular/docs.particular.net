namespace Raven_3
{
    using System;
    using NServiceBus;
    using NServiceBus.Persistence;
    using Raven.Client.Document;

    class ChangeResourceManagerID
    {
        ChangeResourceManagerID(BusConfiguration busConfiguration)
        {
            #region ChangeResourceManagerID

            const string id = "d5723e19-92ad-4531-adad-8611e6e05c8a";
            DocumentStore store = new DocumentStore
            {
                ResourceManagerId = new Guid(id)
            };
            store.Initialize();

            busConfiguration.UsePersistence<RavenDBPersistence>()
                .SetDefaultDocumentStore(store);

            #endregion
        }
    }
}