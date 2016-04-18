namespace Core3.Errors.SecondLevel.ConfigurationSource
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