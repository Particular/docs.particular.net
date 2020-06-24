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

    void OutboxSettingsEx3(EndpointConfiguration endpointConfiguration)
    {
        #region SqlPersistenceOutboxPessimisticMode

        var outboxSettings = endpointConfiguration.EnableOutbox();

        outboxSettings.UsePessimisticConcurrencyControl();

        #endregion
    }

    void OutboxSettingsEx4(EndpointConfiguration endpointConfiguration)
    {
        #region SqlPersistenceOutboxTransactionScopeMode

        var outboxSettings = endpointConfiguration.EnableOutbox();

        outboxSettings.UseTransactionScope();

        #endregion
    }
}
