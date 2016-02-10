namespace Snippets6.Errors.SecondLevel
{
    using NServiceBus;
    using NServiceBus.Features;

    public class DisableWithCode
    {
        public DisableWithCode()
        {
            #region DisableSlrWithCode

            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.DisableFeature<SecondLevelRetries>();

            #endregion
        }

    }
}
