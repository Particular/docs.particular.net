namespace Core5.Recoverability.Delayed.ConfigurationSource
{
    using NServiceBus;

    class Usage
    {
        Usage(BusConfiguration busConfiguration)
        {
            #region SLRConfigurationSourceUsage
            busConfiguration.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}