namespace Core5.UpgradeGuides._5to6
{
    using NServiceBus;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;
    using NServiceBus.Features;

    public class Recoverability
    {

        void DisableDelayedRetries(BusConfiguration busConfiguration)
        {
            #region 5to6-RecoverabilityDisableDelayedRetries

            busConfiguration.DisableFeature<SecondLevelRetries>();

            #endregion
        }

        #region 5to6-RecoverabilityDisableImmediateRetries

        class ConfigTransport :
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

        #endregion
    }
}