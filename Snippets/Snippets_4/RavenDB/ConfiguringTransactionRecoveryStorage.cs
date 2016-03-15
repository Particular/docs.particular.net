namespace Snippets4.RavenDB
{
    using System;
    using System.IO;
    using NServiceBus;
    using NServiceBus.RavenDB;
    using Raven.Client.Document;
    using Raven.Client.Document.DTC;

    class ConfiguringTransactionRecoveryStorage
    {
        public void ManualDtcSettingExample()
        {
            string UrlToRavenDB = "http://localhost:8080";

            #region RavenDBManualDtcSettingExample
            // Value must uniquely identify endpoint on the machine and remain stable on process restarts
            Guid resourceManagerId = new Guid("xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");

            string dtcRecoveryBasePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            string recoveryPath = Path.Combine(dtcRecoveryBasePath, "NServiceBus.RavenDB", resourceManagerId.ToString());

            DocumentStore store = new DocumentStore
            {
                Url = UrlToRavenDB,
                ResourceManagerId = resourceManagerId,
                TransactionRecoveryStorage = new LocalDirectoryTransactionRecoveryStorage(recoveryPath)
            };
            store.Initialize();

            Configure configure = Configure.With();
            configure.RavenDBStorageWithStore(store);
            #endregion
        }
    }
}