namespace Snippets5.Forwarding
{
    using NServiceBus;
    public class ConfigurationSourceUsage
    {
        public ConfigurationSourceUsage()
        {
            #region ConfigurationSourceUsageForMessageForwarding
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}
