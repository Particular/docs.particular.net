namespace Snippets6
{
    using System;
    using NServiceBus;

    public class TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages
    {
        public void Simple()
        {
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            #region TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages

            endpointConfiguration.TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages(TimeSpan.FromMinutes(5));

            #endregion
        }

    }
}