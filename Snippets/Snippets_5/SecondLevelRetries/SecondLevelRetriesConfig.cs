using System;
using NServiceBus;
using NServiceBus.Features;

public class SecondLevelRetriesConfig
{
    public void Disable()
    {
        #region SecondLevelRetriesDisableV5

        Configure.With(b => b.DisableFeature<SecondLevelRetries>());

        #endregion
    }

    public void CustomRetryPolicy()
    {
        #region SecondLevelRetriesCustomRetryPolicyV5

        Configure.With(b => b.SecondLevelRetries().CustomRetryPolicy(MyCustomRetryPolicy));

        #endregion
    }

    TimeSpan MyCustomRetryPolicy(TransportMessage arg)
    {
        return TimeSpan.Zero;
    }
}