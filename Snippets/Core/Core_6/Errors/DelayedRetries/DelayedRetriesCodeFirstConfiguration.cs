namespace Core6.Errors.SecondLevel
{
    using System;
    using NServiceBus;

    public class DelayedRetriesCodeFirstConfiguration
    {
        void ConfigureFlr(EndpointConfiguration endpointConfiguration)
        {
            #region SlrCodeFirstConfiguration

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Delayed(
                delayed =>
                {
                    var retries = delayed.NumberOfRetries(2);
                    retries.TimeIncrease(TimeSpan.FromMinutes(5));
                });
            #endregion
        }
    }
}