﻿namespace Raven_3
{
    using NServiceBus;
    using NServiceBus.Persistence;
    using Raven.Client.Document;
    using Raven.Client.Document.DTC;

    class ConfiguringTransactionRecoveryStorage
    {
        ConfiguringTransactionRecoveryStorage(BusConfiguration busConfiguration)
        {
            #region ConfiguringTransactionRecoveryStorage

            string transactionRecoveryPath = "path to transaction recovery storage unique per endpoint";
            DocumentStore myDocumentStore = new DocumentStore
            {
                TransactionRecoveryStorage = new LocalDirectoryTransactionRecoveryStorage(transactionRecoveryPath)
            };
            // configure document store properties here and initialize

            var persistence = busConfiguration.UsePersistence<RavenDBPersistence>();
            persistence.SetDefaultDocumentStore(myDocumentStore);

            #endregion
        }
    }
}
