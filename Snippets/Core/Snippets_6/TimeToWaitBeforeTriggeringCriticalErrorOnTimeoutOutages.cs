namespace Snippets6
{
    using System;
    using NServiceBus;

    class TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages
    {
        TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages(EndpointConfiguration endpointConfiguration)
        {
            #region TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages

            endpointConfiguration.TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages(TimeSpan.FromMinutes(5));

            #endregion
        }

    }
}