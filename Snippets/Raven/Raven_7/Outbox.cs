namespace Raven_7
{
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
            outboxSettings.SetFrequencyToRunDeduplicationDataCleanup(TimeSpan.FromMinutes(1));
            #endregion
        }

        public void OutboxDisableCleanup(EndpointConfiguration endpointConfiguration)
        {
            #region OutboxRavendBDisableCleanup
            var outbox = endpointConfiguration.EnableOutbox();
            outbox.SetFrequencyToRunDeduplicationDataCleanup(Timeout.InfiniteTimeSpan);
            #endregion
        }
    }
}