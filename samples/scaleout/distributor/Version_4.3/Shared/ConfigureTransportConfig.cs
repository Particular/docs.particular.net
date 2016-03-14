using System;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

#region concurrency
class ConfigureTransportConfig : IProvideConfiguration<TransportConfig>
{
    public TransportConfig GetConfiguration()
    {
        return new TransportConfig
        {
            MaximumConcurrencyLevel = Environment.ProcessorCount
        };
    }
}
#endregion