using NServiceBus;
using Raven.Client.Document;
using Raven.Client.Document.DTC;

class ConfiguringTransactionRecoveryStorage
{
    public void Foo()
    {
        #region ConfiguringTransactionRecoveryStorage

        var transactionRecoveryPath = "path to transaction recovery storage";
            
        Configure.With()
            .CustomiseRavenPersistence(store =>
            {
                var documentStore = ((DocumentStore) store);
                documentStore.TransactionRecoveryStorage = new LocalDirectoryTransactionRecoveryStorage(transactionRecoveryPath);
            });

        #endregion
    }
}
