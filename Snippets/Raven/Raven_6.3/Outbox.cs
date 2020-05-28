namespace Raven_6
{
    using System;
    using System.Threading;
    using NServiceBus;

    class Outbox
    {
        Outbox(EndpointConfiguration endpointConfiguration)
        {
            #region OutboxRavendBTimeToKeep
            var outbox = endpointConfiguration.EnableOutbox();
            outbox.SetTimeToKeepDeduplicationData(TimeSpan.FromDays(7));
            outbox.SetFrequencyToRunDeduplicationDataCleanup(TimeSpan.FromMinutes(1));
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