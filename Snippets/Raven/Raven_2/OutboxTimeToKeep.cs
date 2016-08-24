namespace Raven_2
{
    using System;
    using NServiceBus;
    using NServiceBus.RavenDB.Outbox;

    class OutboxTimeToKeep
    {
        OutboxTimeToKeep(BusConfiguration busConfiguration)
        {
            #region OutboxRavendBTimeToKeep
            busConfiguration.SetTimeToKeepDeduplicationData(TimeSpan.FromDays(7));
            busConfiguration.SetFrequencyToRunDeduplicationDataCleanup(TimeSpan.FromMinutes(1));
            #endregion
        }
    }
}