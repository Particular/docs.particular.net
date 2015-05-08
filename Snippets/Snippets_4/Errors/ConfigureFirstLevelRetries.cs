using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

#region FlrConfiguration
class ConfigureFirstLevelRetries : IProvideConfiguration<TransportConfig>
{
    public TransportConfig GetConfiguration()
    {
        return new TransportConfig
        {
            MaxRetries = 2 
        };
    }
}
#endregion