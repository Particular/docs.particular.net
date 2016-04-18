namespace Core5.Errors.SecondLevel.ConfigurationSource
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