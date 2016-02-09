namespace Snippets6.Errors.FirstLevel.ConfigurationSource
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            EndpointConfiguration configuration = new EndpointConfiguration();

            #region FLRConfigurationSourceUsage
            configuration.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}