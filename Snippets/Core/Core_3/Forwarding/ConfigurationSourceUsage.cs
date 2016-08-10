namespace Core3.Forwarding
{
    using NServiceBus;
    class ConfigurationSourceUsage
    {
        ConfigurationSourceUsage(Configure configure)
        {
            #region ConfigurationSourceUsageForMessageForwarding
            configure.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}
