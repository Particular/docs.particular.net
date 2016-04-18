namespace Core6.Errors.SecondLevel
{
    using NServiceBus;
    using NServiceBus.Features;

    class DisableWithCode
    {
        DisableWithCode(EndpointConfiguration endpointConfiguration)
        {
            #region DisableSlrWithCode
            endpointConfiguration.DisableFeature<SecondLevelRetries>();

            #endregion
        }

    }
}
