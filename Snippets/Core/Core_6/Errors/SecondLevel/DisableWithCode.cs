namespace Core6.Errors.SecondLevel
{
    using NServiceBus;

    class DisableWithCode
    {
        DisableWithCode(EndpointConfiguration endpointConfiguration)
        {
            #region DisableSlrWithCode

            var secondLevelRetries = endpointConfiguration.SecondLevelRetries();
            secondLevelRetries.Disable();

            #endregion
        }

    }
}
