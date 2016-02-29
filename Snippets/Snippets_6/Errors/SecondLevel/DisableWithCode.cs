namespace Snippets6.Errors.SecondLevel
{
    using NServiceBus;
    using NServiceBus.Features;

    public class DisableWithCode
    {
        public DisableWithCode()
        {
            #region DisableSlrWithCode

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.DisableFeature<SecondLevelRetries>();

            #endregion
        }

    }
}
