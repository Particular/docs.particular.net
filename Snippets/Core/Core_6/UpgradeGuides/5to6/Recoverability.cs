namespace Core6.UpgradeGuides._5to6
{
    using System;
    using NServiceBus;

    public class Recoverability
    {
        void ConfigureRetries(EndpointConfiguration endpointConfiguration)
        {
            #region 5to6-RecoverabilityCodeFirstApi
            endpointConfiguration.FirstLevelRetries().NumberOfRetries(3);
            endpointConfiguration.SecondLevelRetries()
                .NumberOfRetries(5)
                .TimeIncrease(TimeSpan.FromSeconds(30));
            #endregion
        }

        void DisableRetries(EndpointConfiguration endpointConfiguration)
        {
            #region 5to6-RecoverabilityDisableRetries
            endpointConfiguration.FirstLevelRetries().Disable();
            endpointConfiguration.SecondLevelRetries().Disable();
            #endregion
        }
    }
}