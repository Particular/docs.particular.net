namespace Snippets_5.RavenDB
{
    using NServiceBus;
    using NServiceBus.Persistence;
    using Raven.Client.Document;
    using Raven.Client.Document.DTC;

    class ConfiguringTransactionRecoveryStorage
    {
        public void Foo()
        {
            #region ConfiguringTransactionRecoveryStorage-V5

            var transactionRecoveryPath = "path to transaction recovery storage";
            var myDocumentStore = new DocumentStore
            {
                TransactionRecoveryStorage = new LocalDirectoryTransactionRecoveryStorage(transactionRecoveryPath)
            };

            var configuration = new BusConfiguration();
            configuration.UsePersistence<RavenDBPersistence>()
                .SetDefaultDocumentStore(myDocumentStore);

            #endregion
        }
    }
}