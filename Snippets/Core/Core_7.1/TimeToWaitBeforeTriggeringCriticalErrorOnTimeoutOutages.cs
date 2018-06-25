namespace Core7
{
    using System;
    using NServiceBus;

    class TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages
    {
        TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages(EndpointConfiguration endpointConfiguration)
        {
            #region TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages

            endpointConfiguration.TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages(
                timeToWait: TimeSpan.FromMinutes(5));

            #endregion
        }

    }
}