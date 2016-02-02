namespace Snippets5.RavenDB
{
    using System;
    using NServiceBus;
    using Raven.Client.Document;

    class ChangeResourceManagerID
    {
        public ChangeResourceManagerID()
        {
            #region ChangeResourceManagerID

            const string id = "d5723e19-92ad-4531-adad-8611e6e05c8a";
            DocumentStore store = new DocumentStore
            {
                ResourceManagerId = new Guid(id)
            };
            store.Initialize();

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UsePersistence<RavenDBPersistence>()
                .SetDefaultDocumentStore(store);

            #endregion
        }
    }
}