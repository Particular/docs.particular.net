namespace Core6.Recoverability.Delayed
{
    using System;
    using NServiceBus;

    class Usage
    {
        void DisableWithCode(EndpointConfiguration endpointConfiguration)
        {
            #region DisableSlrWithCode

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Delayed(
                delayed =>
                {
                    delayed.NumberOfRetries(0);
                });

            #endregion
        }
        void Configure(EndpointConfiguration endpointConfiguration)
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