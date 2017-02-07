namespace Core4.Recoverability.Delayed.ConfigurationSource
{
    using NServiceBus;

    class Usage
    {
        Usage(Configure configure)
        {
            #region DelayedRetriesConfigurationSourceUsage

            configure.CustomConfigurationSource(new ConfigurationSource());

            #endregion
        }
    }
}