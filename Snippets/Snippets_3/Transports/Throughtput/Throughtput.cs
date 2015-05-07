using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;
using NServiceBus.Unicast;

public class Throughtput
{

    #region ThroughtputFromCode

    public class ConfigureEncryption :
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