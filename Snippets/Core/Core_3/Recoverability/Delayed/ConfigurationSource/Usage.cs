namespace Core3.Recoverability.Delayed.ConfigurationSource
{
    using NServiceBus;

    class Usage
    {
        Usage(Configure configure)
        {
            #region FLRConfigurationSourceUsage

            configure.CustomConfigurationSource(new ConfigurationSource());

            #endregion
        }
    }
}