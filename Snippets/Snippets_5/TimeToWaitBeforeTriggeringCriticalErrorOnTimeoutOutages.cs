using System;
using NServiceBus;


public class TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages
{
    public void Simple()
    {
        #region TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages

        BusConfiguration configuration = new BusConfiguration();

        configuration.TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages(TimeSpan.FromMinutes(5));

        #endregion
    }

}