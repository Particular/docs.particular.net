namespace Snippets4.Forwarding
{
    using NServiceBus;
    public class ConfigurationSourceUsage
    {
        public ConfigurationSourceUsage()
        {
            #region ConfigurationSourceUsageForMessageForwarding
            Configure configure = Configure.With();
            configure.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}
