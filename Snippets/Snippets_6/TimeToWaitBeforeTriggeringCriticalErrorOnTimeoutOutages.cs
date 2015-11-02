namespace Snippets6
{
    using System;
    using NServiceBus;

    public class TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages
    {
        public void Simple()
        {
            #region TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages

            BusConfiguration busConfiguration = new BusConfiguration();

            busConfiguration.TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages(TimeSpan.FromMinutes(5));

            #endregion
        }

    }
}