namespace Core6.Errors.FirstLevel
{
    using NServiceBus;

    class DisableWithCode
    {
        DisableWithCode(EndpointConfiguration endpointConfiguration)
        {
            #region DisableFlrWithCode
            endpointConfiguration.DisableFirstLevelRetries();

            #endregion
        }

    }
}
