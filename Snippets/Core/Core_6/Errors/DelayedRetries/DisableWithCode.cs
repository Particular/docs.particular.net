namespace Core6.Errors.SecondLevel
{
    using NServiceBus;

    class DisableWithCode
    {
        DisableWithCode(EndpointConfiguration endpointConfiguration)
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

    }
}