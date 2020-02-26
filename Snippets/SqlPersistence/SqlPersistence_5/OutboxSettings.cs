using System;
using NServiceBus;

class OutboxSettings
{
    void OutboxSettingsEx1(EndpointConfiguration endpointConfiguration)
    {
        #region SqlPersistenceOutboxSettings

        var outboxSettings = endpointConfiguration.EnableOutbox();

        outboxSettings.KeepDeduplicationDataFor(TimeSpan.FromDays(6));
        outboxSettings.RunDeduplicationDataCleanupEvery(TimeSpan.FromMinutes(15));

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
