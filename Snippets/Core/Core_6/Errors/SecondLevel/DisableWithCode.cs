namespace Core6.Errors.SecondLevel
{
    using NServiceBus;

    class DisableWithCode
    {
        DisableWithCode(EndpointConfiguration endpointConfiguration)
        {
            #region DisableSlrWithCode
            endpointConfiguration.SecondLevelRetries().Disable();

            #endregion
        }

    }
}
