namespace Core5.Recoverability.Delayed
{
    using NServiceBus;
    using NServiceBus.Features;

    class Usage
    {
        void DisableWithCode(BusConfiguration busConfiguration)
        {
            #region DisableSlrWithCode

            busConfiguration.DisableFeature<SecondLevelRetries>();

            #endregion
        }

    }
}
