namespace Snippets6.Errors.SecondLevel
{
    using NServiceBus;
    using NServiceBus.Features;

    public class DisableWithCode
    {
        public DisableWithCode(EndpointConfiguration endpointConfiguration)
        {
            #region DisableSlrWithCode
            endpointConfiguration.DisableFeature<SecondLevelRetries>();

            #endregion
        }

    }
}
