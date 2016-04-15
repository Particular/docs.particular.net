namespace Snippets3.Transports.Throughput
{
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #region TuningFromCode

    public class ProvideConfiguration :
        IProvideConfiguration<MsmqTransportConfig>
    {
        public MsmqTransportConfig GetConfiguration()
        {
            return new MsmqTransportConfig
            {
                NumberOfWorkerThreads = 5
            };
        }
    }

    #endregion

}