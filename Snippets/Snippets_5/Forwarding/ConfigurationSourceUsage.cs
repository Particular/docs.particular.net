namespace Snippets5.Forwarding
{
    using NServiceBus;
    class ConfigurationSourceUsage
    {
        ConfigurationSourceUsage(BusConfiguration busConfiguration)
        {
            #region ConfigurationSourceUsageForMessageForwarding
            busConfiguration.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}
