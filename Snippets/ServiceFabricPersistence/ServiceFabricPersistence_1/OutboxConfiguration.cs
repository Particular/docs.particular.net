using System;
using NServiceBus;

class OutboxConfiguration
{
    void OutboxConfigurationUsage(EndpointConfiguration endpointConfiguration)
    {
        #region ServiceFabricPersistenceOutboxConfiguration

        var outboxSettings = endpointConfiguration.EnableOutbox();
        outboxSettings.SetTimeToKeepDeduplicationData(TimeSpan.FromDays(1));
        outboxSettings.SetFrequencyToRunDeduplicationDataCleanup(TimeSpan.FromMinutes(1));

        #endregion
    }
}