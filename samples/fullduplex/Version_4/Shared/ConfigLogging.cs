using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

class ConfigLogging : IProvideConfiguration<Logging>
{
    public Logging GetConfiguration()
    {
        return new Logging
               {
                   Threshold = "Info"
               };
    }
}