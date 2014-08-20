using System;
using NServiceBus;
using NServiceBus.Management.Retries;
using NServiceBus.Management.Retries.Helpers;
using NServiceBus.Unicast.Transport;

public class SecondLevelRetriesConfig
{
    public void Simple()
    {
        #region SecondLevelRetriesDisableV3

        Configure.Instance.DisableSecondLevelRetries();

        #endregion

        #region SecondLevelRetriesCustomPolicyV3

        SecondLevelRetries.RetryPolicy = MyCustomRetryPolicy;

        #endregion
    }

    #region SecondLevelRetriesCustomPolicyHandlerV3
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