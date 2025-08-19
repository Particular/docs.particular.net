using System;
using System.Threading;
using NServiceBus;

class Outbox
{
    Outbox(EndpointConfiguration endpointConfiguration)
    {
        #region OutboxRavendBTimeToKeep
        var outboxSettings = endpointConfiguration.EnableOutbox();
        outboxSettings.SetTimeToKeepDeduplicationData(TimeSpan.FromDays(7));
        #endregion
    }

    void OutboxDisableCleanup(EndpointConfiguration endpointConfiguration)
    {
        #region OutboxRavendBDisableCleanup
        var outbox = endpointConfiguration.EnableOutbox();
        outbox.SetFrequencyToRunDeduplicationDataCleanup(Timeout.InfiniteTimeSpan);
        #endregion
    }

    void Cleaner(EndpointConfiguration endpointConfiguration)
    {
        #region OutboxRavendBEnableCleaner
        var outboxSettings = endpointConfiguration.EnableOutbox();
        outboxSettings.SetFrequencyToRunDeduplicationDataCleanup(TimeSpan.FromMinutes(1));
        #endregion
    }
}