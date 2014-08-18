using System;
using NServiceBus;

public class SecondLevelRetries
{
    public void CustomRetryPolicy()
    {
        #region SecondLevelRetriesCustomRetryPolicyV5

        Configure.With()
            .SecondLevelRetries(s => s.CustomRetryPolicy(MyCustomRetryPolicy));

        #endregion
    }

    TimeSpan MyCustomRetryPolicy(TransportMessage arg)
    {
        return TimeSpan.Zero;
    }
}