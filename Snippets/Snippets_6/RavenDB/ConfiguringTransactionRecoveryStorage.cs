namespace Snippets5.RavenDB
{
    using NServiceBus;
    using Raven.Client.Document;
    using Raven.Client.Document.DTC;

    class ConfiguringTransactionRecoveryStorage
    {
        public void Foo()
        {
            #region ConfiguringTransactionRecoveryStorage

            string transactionRecoveryPath = "path to transaction recovery storage unique per endpoint";
            DocumentStore myDocumentStore = new DocumentStore
            {
                TransactionRecoveryStorage = new LocalDirectoryTransactionRecoveryStorage(transactionRecoveryPath)
            };
            // configure document store properties here and initialize

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UsePersistence<RavenDBPersistence>()
                .SetDefaultDocumentStore(myDocumentStore);

            #endregion
        }
    }
}
