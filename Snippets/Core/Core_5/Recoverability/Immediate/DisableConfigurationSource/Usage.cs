namespace Core5.Recoverability.Immediate.ConfigurationSource
{
    using NServiceBus;

    class DisableUsage
    {
        DisableUsage(BusConfiguration busConfiguration)
        {
            #region DisableImmediateRetriesConfigurationSourceUsage
            busConfiguration.CustomConfigurationSource(new DisableConfigurationSource());
            #endregion
        }
    }
}