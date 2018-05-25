using System;
using NServiceBus;
using NServiceBus.InMemory.Outbox;

class OutboxSettings
{
    void OutboxSettingsEx1(EndpointConfiguration endpointConfiguration)
    {
        #region SqlPersistenceOutboxSettings

        var outboxSettings = endpointConfiguration.EnableOutbox();

        outboxSettings.KeepDeduplicationDataFor(TimeSpan.FromDays(6));
        outboxSettings.TimeToKeepDeduplicationData(TimeSpan.FromMinutes(15));

        #endregion
    }

    void OutboxSettingsEx2(EndpointConfiguration endpointConfiguration)
    {
        #region SqlPersistenceOutboxDisableCleanup

        var outboxSettings = endpointConfiguration.EnableOutbox();

        outboxSettings.DisableCleanup();

        #endregion
    }
}
