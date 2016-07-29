namespace Core6.Errors.SecondLevel
{
    using System;
    using NServiceBus;

    public class DelayedRetriesCodeFirstConfiguration
    {
        void ConfigureFlr(EndpointConfiguration endpointConfiguration)
        {
            #region SlrCodeFirstConfiguration

            var recoverabilitySettings = endpointConfiguration.Recoverability();
            recoverabilitySettings.Delayed(
                delayed =>
                {
                    var numberOfRetries = delayed.NumberOfRetries(2);
                    numberOfRetries.TimeIncrease(TimeSpan.FromMinutes(5));
                });

            #endregion
        }
    }
}