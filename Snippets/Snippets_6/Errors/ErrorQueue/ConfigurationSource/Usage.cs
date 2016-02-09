namespace Snippets6.Errors.ErrorQueue.ConfigurationSource
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            EndpointConfiguration configuration = new EndpointConfiguration();

            #region UseCustomConfigurationSourceForErrorQueueConfig
            configuration.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}