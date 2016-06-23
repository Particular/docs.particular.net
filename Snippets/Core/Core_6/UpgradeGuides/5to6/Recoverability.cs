namespace Core6.UpgradeGuides._5to6
{
    using System;
    using NServiceBus;

    public class Recoverability
    {
        void ConfigureRetries(EndpointConfiguration endpointConfiguration)
        {
            #region 5to6-RecoverabilityCodeFirstApi

            var firstLevelRetries = endpointConfiguration.FirstLevelRetries();
            firstLevelRetries.NumberOfRetries(3);
            var secondLevelRetries = endpointConfiguration.SecondLevelRetries();
            secondLevelRetries.NumberOfRetries(5);
            secondLevelRetries.TimeIncrease(TimeSpan.FromSeconds(30));
            #endregion
        }

        void DisableRetries(EndpointConfiguration endpointConfiguration)
        {
            #region 5to6-RecoverabilityDisableRetries

            var firstLevelRetries = endpointConfiguration.FirstLevelRetries();
            firstLevelRetries.Disable();

            var secondLevelRetries = endpointConfiguration.SecondLevelRetries();
            secondLevelRetries.Disable();
            #endregion
        }
    }
}