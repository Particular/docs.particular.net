using System;
using System.IO;
using NServiceBus;
using NServiceBus.RavenDB;
using Raven.Client.Document;
using Raven.Client.Document.DTC;

class CustomRavenConfig
{
    CustomRavenConfig(Configure configure)
    {
        #region OldRavenDBPersistenceInitialization

        configure.RavenPersistence();
        configure.RavenSagaPersister();
        configure.RavenSubscriptionStorage();
        configure.UseRavenTimeoutPersister();
        configure.UseRavenGatewayDeduplication();
        configure.UseRavenGatewayPersister();

        #endregion

        #region Version2_5RavenDBPersistenceInitialization

        // Need to call this method
        configure.RavenDBStorage();
        // Call this method to use Raven saga storage
        configure.UseRavenDBSagaStorage();
        // Call this method to use Raven subscription storage
        configure.UseRavenDBSubscriptionStorage();
        // Call this method to use Raven timeout storage
        configure.UseRavenDBTimeoutStorage();
        // Call this method to use Raven deduplication storage for the Gateway
        configure.UseRavenDBGatewayDeduplicationStorage();
        // Call this method to use the  Raven Gateway storage method
        configure.UseRavenDBGatewayStorage();

        #endregion
    }

    void ManualDtcSettingExample(Configure configure)
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

        configure.RavenDBStorageWithStore(store);

        #endregion
    }

}