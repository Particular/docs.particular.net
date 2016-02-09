namespace Snippets6
{
    using System;
    using NServiceBus;

    public class TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages
    {
        public void Simple()
        {
            #region TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages

            EndpointConfiguration configuration = new EndpointConfiguration();

            configuration.TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages(TimeSpan.FromMinutes(5));

            #endregion
        }

    }
}