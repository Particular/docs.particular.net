using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

class DisableImmediateRetries :
    IProvideConfiguration<TransportConfig>
{
    public TransportConfig GetConfiguration()
    {
        return new TransportConfig
        {
            MaxRetries = 0
        };
    }
}