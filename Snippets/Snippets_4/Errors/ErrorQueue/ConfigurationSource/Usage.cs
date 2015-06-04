namespace Snippets4.Errors.ErrorQueue.ConfigurationSource
{
    using NServiceBus;

    public class Usage 
    {
        public Usage()
        {
            #region UseCustomConfigurationSourceForErrorQueueConfig
            Configure.With()
                .CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}