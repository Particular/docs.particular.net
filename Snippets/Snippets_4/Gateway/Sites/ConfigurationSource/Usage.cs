namespace Snippets4.Gateway.Sites.ConfigurationSource
{
    using NServiceBus;

    public class Usage 
    {
        public Usage()
        {
            Configure configure = Configure.With();

            #region UseCustomConfigurationSourceForGatewaySitesConfig
            configure.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}