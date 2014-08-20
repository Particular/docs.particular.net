using System;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.SecondLevelRetries.Helpers;

public class SecondLevelRetriesConfig
{
    public void Simple()
    {
        #region SecondLevelRetriesDisableV4

        Configure.Features.Disable<SecondLevelRetries>();

        #endregion

        #region SecondLevelRetriesCustomPolicyV4

        Configure.Features.SecondLevelRetries(settings => settings.CustomRetryPolicy(MyCustomRetryPolicy));

        #endregion
    }

    #region SecondLevelRetriesCustomPolicyHandlerV4
    TimeSpan MyCustomRetryPolicy(TransportMessage message)
    {
        // retry max 3 times
        if (TransportMessageHelpers.GetNumberOfRetries(message) >= 3)
        {
            // sending back a TimeSpan.MinValue tells the 
            // SecondLevelRetry not to retry this message
            return TimeSpan.MinValue;
        }

        return TimeSpan.FromSeconds(5);
    }
    #endregion

}