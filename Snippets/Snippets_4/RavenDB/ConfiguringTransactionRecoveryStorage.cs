namespace Snippets4.RavenDB
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

            Configure configure = Configure.With();
            configure.CustomiseRavenPersistence(store =>
            {
                DocumentStore documentStore = ((DocumentStore) store);
                documentStore.TransactionRecoveryStorage = new LocalDirectoryTransactionRecoveryStorage(transactionRecoveryPath);
            });

            #endregion
        }
    }
}