using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

class ConfigTimeoutManager : IProvideConfiguration<UnicastBusConfig>
{
    public UnicastBusConfig GetConfiguration()
    {
        return new UnicastBusConfig
        {
            TimeoutManagerAddress = "TimeoutRouter"
        };
    }
}