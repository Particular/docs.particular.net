using System;
using NServiceBus;

class OutboxConfiguration
{
    void OutboxConfigurationUsage(EndpointConfiguration endpointConfiguration)
    {
        #region ServiceFabricPersistenceOutboxConfiguration

        var outbox = endpointConfiguration.EnableOutbox();
        outbox.SetTimeToKeepDeduplicationData(TimeSpan.FromDays(1));
        outbox.SetFrequencyToRunDeduplicationDataCleanup(TimeSpan.FromMinutes(1));

        #endregion
    }
}