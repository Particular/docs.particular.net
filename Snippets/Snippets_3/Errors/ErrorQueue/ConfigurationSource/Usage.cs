namespace Snippets3.Errors.ErrorQueue.ConfigurationSource
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