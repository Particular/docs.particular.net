namespace Snippets5
{
    using System;
    using NServiceBus;

    public class TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages
    {
        public void Simple()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages
            
            busConfiguration.TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages(TimeSpan.FromMinutes(5));

            #endregion
        }

    }
}