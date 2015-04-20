using System;
using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;

public class TimeToKeep
{
    public void Customize()
    {
        BusConfiguration configuration = null;

        #region OutboxRavendBTimeToKeep
        configuration.GetSettings().Set("Outbox.TimeToKeepDeduplicationData", TimeSpan.FromDays(7));
        configuration.GetSettings().Set("Outbox.FrequencyToRunDeduplicationDataCleanup", TimeSpan.FromMinutes(1));
        #endregion
    }
}