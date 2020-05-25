namespace Raven_6
{
    using System;
    using NServiceBus;

    class Upgrade
    {
        Upgrade(EndpointConfiguration endpointConfiguration)
        {
            #region OutboxSettingsUpgrade
            var outbox = endpointConfiguration.EnableOutbox();
            outbox.SetTimeToKeepDeduplicationData(TimeSpan.FromDays(7));
            outbox.SetFrequencyToRunDeduplicationDataCleanup(TimeSpan.FromMinutes(1));
            #endregion
        }
    }
}