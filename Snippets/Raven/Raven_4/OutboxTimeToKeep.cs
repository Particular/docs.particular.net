namespace Raven_4
{
    using System;
    using NServiceBus;

    class OutboxTimeToKeep
    {
        OutboxTimeToKeep(EndpointConfiguration endpointConfiguration)
        {
            #region OutboxRavendBTimeToKeep
            endpointConfiguration.SetTimeToKeepDeduplicationData(TimeSpan.FromDays(7));
            endpointConfiguration.SetFrequencyToRunDeduplicationDataCleanup(TimeSpan.FromMinutes(1));
            #endregion
        }
    }
}