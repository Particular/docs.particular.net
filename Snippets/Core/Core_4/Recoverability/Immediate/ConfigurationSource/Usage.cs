namespace Core4.Recoverability.Immediate.ConfigurationSource
{
    using NServiceBus;

    class Usage
    {
        Usage(Configure configure)
        {
            #region SLRConfigurationSourceUsage

            configure.CustomConfigurationSource(new ConfigurationSource());

            #endregion
        }
    }
}