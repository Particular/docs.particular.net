using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;
using NServiceBus.Unicast;

public class Throughtput
{

    #region ThroughtputFromCode

    public class ConfigureEncryption :
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
    public void WithCode()
    {
        UnicastBus bus = null;

        #region ChangeThroughtput
        //bus is an instance of NServiceBus.Unicast.UnicastBus
        bus.Transport.ChangeMaximumMessageThroughputPerSecond(10);
        bus.Transport.ChangeMaximumConcurrencyLevel(5);
        #endregion

        #region ReadThroughtput

        //bus is an instance of NServiceBus.Unicast.UnicastBus
        int messageThroughputPerSecond = bus.Transport.MaximumMessageThroughputPerSecond;
        int maximumConcurrencyLevel = bus.Transport.MaximumConcurrencyLevel;

        #endregion
    }

}