namespace Core6.Errors.SecondLevel
{
    using System;
    using NServiceBus;

    public class SlrCodeFirstConfiguration
    {
        void ConfigureFlr(EndpointConfiguration endpointConfiguration)
        {
            #region SlrCodeFirstConfiguration

            var recoverabilitySettings = endpointConfiguration.Recoverability();
            recoverabilitySettings.Delayed(delayed => delayed.NumberOfRetries(2).TimeIncrease(TimeSpan.FromMinutes(5)));
            #endregion
        }
    }
}