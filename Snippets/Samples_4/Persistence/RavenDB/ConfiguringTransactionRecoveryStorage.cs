namespace Snippets_5.RavenDB
{
    using NServiceBus;
    using Raven.Client.Document;
    using Raven.Client.Document.DTC;

    class ConfiguringTransactionRecoveryStorage
    {
        public void Foo()
        {
            #region ConfiguringTransactionRecoveryStorage-V4

            var transactionRecoveryPath = "path to transaction recovery storage";
            
            Configure.With()
                .CustomiseRavenPersistence(store =>
                {
                    ((DocumentStore) store).TransactionRecoveryStorage = new LocalDirectoryTransactionRecoveryStorage(transactionRecoveryPath);
                });

            #endregion
        }
    }
}