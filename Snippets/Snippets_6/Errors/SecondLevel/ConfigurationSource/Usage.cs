namespace Snippets6.Errors.SecondLevel.ConfigurationSource
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            EndpointConfiguration configuration = new EndpointConfiguration();

            #region SLRConfigurationSourceUsage
            configuration.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}