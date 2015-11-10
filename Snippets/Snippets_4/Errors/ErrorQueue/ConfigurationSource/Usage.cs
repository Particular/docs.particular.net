namespace Snippets4.Errors.ErrorQueue.ConfigurationSource
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region UseCustomConfigurationSourceForErrorQueueConfig

            Configure configure = Configure.With();
            configure.CustomConfigurationSource(new ConfigurationSource());

            #endregion
        }
    }
}