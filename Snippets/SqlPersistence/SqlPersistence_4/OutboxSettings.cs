namespace SqlPersistence_4
{
    using NServiceBus;

    class OutboxSettings
    {
        void OutboxSettingsEx1(EndpointConfiguration endpointConfiguration)
        {
            #region SqlPersistenceOutboxSettings 4.1

            var outboxSettings = endpointConfiguration.EnableOutbox();
            //TODO: uncomment after updating dependency to 4.1
            //outboxSettings.SetTimeToKeepDeduplicationData(TimeSpan.FromDays(6));
            //outboxSettings.SetFrequencyToRunDeduplicationDataCleanup(TimeSpan.FromMinutes(15));

            #endregion
        }

        void OutboxSettingsEx2(EndpointConfiguration endpointConfiguration)
        {
            #region SqlPersistenceOutboxDisableCleanup 4.1

            var outboxSettings = endpointConfiguration.EnableOutbox();
            //TODO: uncomment after updating dependency to 4.1
            //outboxSettings.DisableCleanup();

            #endregion
        }
    }
}
