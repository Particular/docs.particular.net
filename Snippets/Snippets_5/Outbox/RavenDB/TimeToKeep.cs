using System;
using NServiceBus;
using NServiceBus.RavenDB.Outbox;

public class TimeToKeep
{
    public void Customize()
    {
        BusConfiguration configuration = null;

        #region OutboxRavendBTimeToKeep
        configuration.SetTimeToKeepDeduplicationData(TimeSpan.FromDays(7));
        configuration.SetFrequencyToRunDeduplicationDataCleanup(TimeSpan.FromMinutes(1));
        #endregion
    }
}