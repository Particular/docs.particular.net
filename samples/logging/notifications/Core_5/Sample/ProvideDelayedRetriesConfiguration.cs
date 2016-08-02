using System;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

#region customDelayedRetries
class ProvideDelayedRetriesConfiguration :
    IProvideConfiguration<SecondLevelRetriesConfig>
{
    public SecondLevelRetriesConfig GetConfiguration()
    {
        return new SecondLevelRetriesConfig
        {
            TimeIncrease = TimeSpan.FromSeconds(1)
        };
    }
}
#endregion