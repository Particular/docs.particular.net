namespace Core5.Recoverability.Immediate.ConfigurationSource
{
    using NServiceBus;

    class Usage
    {
        Usage(BusConfiguration busConfiguration)
        {
            #region FLRConfigurationSourceUsage
            busConfiguration.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}