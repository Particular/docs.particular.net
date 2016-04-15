namespace Snippets5.Errors.ErrorQueue.ConfigurationSource
{
    using NServiceBus;

    class Usage 
    {
        Usage(BusConfiguration busConfiguration)
        {
            #region UseCustomConfigurationSourceForErrorQueueConfig
            busConfiguration.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}