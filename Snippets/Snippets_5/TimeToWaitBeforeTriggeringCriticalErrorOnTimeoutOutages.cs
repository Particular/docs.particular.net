using System;
using NServiceBus;


public class TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages
{
    public void Simple()
    {
        #region TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages 5

        var configuration = new BusConfiguration();

        configuration.TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages(TimeSpan.FromMinutes(5));

        #endregion
    }

}