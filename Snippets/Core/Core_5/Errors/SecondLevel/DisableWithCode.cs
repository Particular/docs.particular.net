namespace Core5.Errors.SecondLevel
{
    using NServiceBus;
    using NServiceBus.Features;

    class DisableWithCode
    {
        DisableWithCode(BusConfiguration busConfiguration)
        {
            #region DisableSlrWithCode

            busConfiguration.DisableFeature<SecondLevelRetries>();

            #endregion
        }

    }
}
