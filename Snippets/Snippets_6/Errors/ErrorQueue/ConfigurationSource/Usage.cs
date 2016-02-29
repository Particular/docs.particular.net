namespace Snippets6.Errors.ErrorQueue.ConfigurationSource
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();

            #region UseCustomConfigurationSourceForErrorQueueConfig
            endpointConfiguration.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}