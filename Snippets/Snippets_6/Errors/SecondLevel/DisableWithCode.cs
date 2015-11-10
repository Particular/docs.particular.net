namespace Snippets6.Errors.SecondLevel
{
    using NServiceBus;
    using NServiceBus.Features;

    public class DisableWithCode
    {
        public DisableWithCode()
        {
            #region DisableSlrWithCode

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.DisableFeature<SecondLevelRetries>();

            #endregion
        }

    }
}
