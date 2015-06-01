using System;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;


#region SlrConfiguration
class ConfigureSecondLevelRetries : IProvideConfiguration<SecondLevelRetriesConfig>
{
    public SecondLevelRetriesConfig GetConfiguration()
    {
        return new SecondLevelRetriesConfig
        {
            Enabled = true,
            NumberOfRetries = 2,
            TimeIncrease = TimeSpan.FromSeconds(10)
        };
    }
}
#endregion
