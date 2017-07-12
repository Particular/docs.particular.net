namespace Raven_3
{
    using System;
    using NServiceBus;
    using NServiceBus.RavenDB.Outbox;
    using Raven.Client.Document;

    class Outbox
    {
        Outbox(BusConfiguration busConfiguration)
        {
            #region OutboxRavendBTimeToKeep
            busConfiguration.SetTimeToKeepDeduplicationData(TimeSpan.FromDays(7));
            busConfiguration.SetFrequencyToRunDeduplicationDataCleanup(TimeSpan.FromMinutes(1));
            #endregion

            // ReSharper disable once UseObjectOrCollectionInitializer
            var documentStore = new DocumentStore();

            #region OutboxDisableDocStoreDtc
            documentStore.EnlistInDistributedTransactions = false;
            #endregion
        }
    }
}