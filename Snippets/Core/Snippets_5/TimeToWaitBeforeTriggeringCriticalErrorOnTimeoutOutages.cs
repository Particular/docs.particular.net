namespace Snippets5
{
    using System;
    using NServiceBus;

    class TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages
    {
        TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages(BusConfiguration busConfiguration)
        {
            #region TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages
            
            busConfiguration.TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages(TimeSpan.FromMinutes(5));

            #endregion
        }

    }
}