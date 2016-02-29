namespace Snippets6
{
    using System;
    using NServiceBus;

    public class TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages
    {
        public void Simple()
        {
            #region TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();

            endpointConfiguration.TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages(TimeSpan.FromMinutes(5));

            #endregion
        }

    }
}