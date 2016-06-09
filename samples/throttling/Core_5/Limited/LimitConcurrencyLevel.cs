using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

#region LimitConcurrency
public class LimitConcurrencyLevel :
    IProvideConfiguration<TransportConfig>
{
    public TransportConfig GetConfiguration()
    {
        return new TransportConfig
        {
            MaximumConcurrencyLevel = 1,
        };
    }
}
#endregion