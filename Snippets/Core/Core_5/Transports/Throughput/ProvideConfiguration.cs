namespace Core5.Transports.Throughput
{
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #region TuningFromConfigurationProvider

    public class ProvideConfiguration :
        IProvideConfiguration<TransportConfig>
    {
        public TransportConfig GetConfiguration()
        {
            return new TransportConfig
            {
                MaximumConcurrencyLevel = 5,
                MaximumMessageThroughputPerSecond = 10
            };
        }
    }

    #endregion
}