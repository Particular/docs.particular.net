namespace Core6.Errors.SecondLevel
{
    using NServiceBus;

    class DisableWithCode
    {
        DisableWithCode(EndpointConfiguration endpointConfiguration)
        {
            #region DisableSlrWithCode

            var recoverabilitySettings = endpointConfiguration.Recoverability();
            recoverabilitySettings.Delayed(
                delayed =>
                {
                    delayed.NumberOfRetries(0);
                });

            #endregion
        }

    }
}