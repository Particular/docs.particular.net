namespace Raven_6
{
    using System;
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
    }
}