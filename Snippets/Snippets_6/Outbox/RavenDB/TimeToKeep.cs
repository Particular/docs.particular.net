namespace Snippets6.Outbox.RavenDB
{
    using System;
    using NServiceBus;
    using NServiceBus.RavenDB.Outbox;

    public class TimeToKeep
    {
        public TimeToKeep()
        {
            EndpointConfiguration endpointConfiguration = null;

            #region OutboxRavendBTimeToKeep
            endpointConfiguration.SetTimeToKeepDeduplicationData(TimeSpan.FromDays(7));
            endpointConfiguration.SetFrequencyToRunDeduplicationDataCleanup(TimeSpan.FromMinutes(1));
            #endregion
        }
    }
}