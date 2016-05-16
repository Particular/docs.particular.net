namespace Core6.Outbox.RavenDB
{
    using System;
    using NServiceBus;
    
    class TimeToKeep
    {
        TimeToKeep(EndpointConfiguration endpointConfiguration)
        {
            #region OutboxRavendBTimeToKeep
            endpointConfiguration.SetTimeToKeepDeduplicationData(TimeSpan.FromDays(7));
            endpointConfiguration.SetFrequencyToRunDeduplicationDataCleanup(TimeSpan.FromMinutes(1));
            #endregion
        }
    }
}