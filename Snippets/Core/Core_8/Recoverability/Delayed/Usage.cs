namespace Core8.Recoverability.Delayed
{
    using System;
    using NServiceBus;

    class Usage
    {
        void DisableWithCode(EndpointConfiguration endpointConfiguration)
        {
            #region DisableDelayedRetries

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
            #region DelayedRetriesConfiguration

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Delayed(
                delayed =>
                {
                    delayed.NumberOfRetries(2);
                    delayed.TimeIncrease(TimeSpan.FromMinutes(5));
                });
            #endregion
        }

    }
}
