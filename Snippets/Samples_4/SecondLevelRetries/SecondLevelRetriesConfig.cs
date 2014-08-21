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

        Configure.Features.SecondLevelRetries(s => s.CustomRetryPolicy(MyCustomRetryPolicy));

        #endregion
    }

    #region SecondLevelRetriesCustomPolicyHandlerV4
    TimeSpan MyCustomRetryPolicy(TransportMessage message)
    {
        // retry max 3 times
        if (GetNumberOfRetries(message) >= 3)
        {
            // sending back a TimeSpan.MinValue tells the 
            // SecondLevelRetry not to retry this message
            return TimeSpan.MinValue;
        }

        return TimeSpan.FromSeconds(5);
    }

    static int GetNumberOfRetries(TransportMessage message)
    {
        string value;
        if (message.Headers.TryGetValue(Headers.Retries, out value))
        {
            int i;
            if (int.TryParse(value, out i))
            {
                return i;
            }
        }
        return 0;
    }
    #endregion

}