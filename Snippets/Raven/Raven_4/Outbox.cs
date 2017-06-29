namespace Raven_4
{
    using System;
    using NServiceBus;
    using Raven.Client.Document;

    class Outbox
    {
        Outbox(EndpointConfiguration endpointConfiguration)
        {
            #region OutboxRavendBTimeToKeep
            endpointConfiguration.SetTimeToKeepDeduplicationData(TimeSpan.FromDays(7));
            endpointConfiguration.SetFrequencyToRunDeduplicationDataCleanup(TimeSpan.FromMinutes(1));
            #endregion

            // ReSharper disable once UseObjectOrCollectionInitializer
            var documentStore = new DocumentStore();

            #region OutboxDisableDocStoreDtc
            documentStore.EnlistInDistributedTransactions = false;
            #endregion
        }
    }
}