using System;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

#region concurrency
class ConfigureMsmqTransportConfig : IProvideConfiguration<MsmqTransportConfig>
{
    public MsmqTransportConfig GetConfiguration()
    {
        return new MsmqTransportConfig
        {
            NumberOfWorkerThreads = Environment.ProcessorCount
        };
    }
}
#endregion