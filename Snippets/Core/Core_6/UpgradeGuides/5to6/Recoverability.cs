namespace Core6.UpgradeGuides._5to6
{
    using System;
    using NServiceBus;

    public class Recoverability
    {
        void ConfigureRetries(EndpointConfiguration endpointConfiguration)
        {
            #region 5to6-RecoverabilityCodeFirstApi

            var recoverabilitySettings = endpointConfiguration.Recoverability();
            recoverabilitySettings.Immediate(immediate => immediate.NumberOfRetries(3));
            recoverabilitySettings.Delayed(delayed=> delayed.NumberOfRetries(5).TimeIncrease(TimeSpan.FromSeconds(30)));
            #endregion
        }

        void DisableRetries(EndpointConfiguration endpointConfiguration)
        {
            #region 5to6-RecoverabilityDisableRetries

            var recoverabilitySettings = endpointConfiguration.Recoverability();
            recoverabilitySettings.Immediate(immediate => immediate.NumberOfRetries(0));
            recoverabilitySettings.Delayed(delayed => delayed.NumberOfRetries(0));
            #endregion
        }
    }
}