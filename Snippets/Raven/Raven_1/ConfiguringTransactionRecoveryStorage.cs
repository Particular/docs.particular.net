using NServiceBus;
using Raven.Client.Document;
using Raven.Client.Document.DTC;

class ConfiguringTransactionRecoveryStorage
{
    ConfiguringTransactionRecoveryStorage(Configure configure)
    {
        #region ConfiguringTransactionRecoveryStorage

        string transactionRecoveryPath = "path to transaction recovery storage unique per endpoint";
        configure.CustomiseRavenPersistence(store =>
        {
            DocumentStore documentStore = (DocumentStore) store;
            documentStore.TransactionRecoveryStorage = new LocalDirectoryTransactionRecoveryStorage(transactionRecoveryPath);
        });

        #endregion
    }
}