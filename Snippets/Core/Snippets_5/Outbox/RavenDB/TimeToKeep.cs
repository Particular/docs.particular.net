namespace Snippets5.Outbox.RavenDB
{
    using System;
    using NServiceBus;
    using NServiceBus.RavenDB.Outbox;

    class TimeToKeep
    {
        TimeToKeep(BusConfiguration busConfiguration)
        {
            #region OutboxRavendBTimeToKeep
            busConfiguration.SetTimeToKeepDeduplicationData(TimeSpan.FromDays(7));
            busConfiguration.SetFrequencyToRunDeduplicationDataCleanup(TimeSpan.FromMinutes(1));
            #endregion
        }
    }
}