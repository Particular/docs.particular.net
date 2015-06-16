namespace Snippets6.Errors.ErrorQueue.ConfigurationSource
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            BusConfiguration busConfiguration = new BusConfiguration();

            #region UseCustomConfigurationSourceForErrorQueueConfig
            busConfiguration.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}