namespace Core3.Recoverability.Immediate.ConfigurationSource
{
    using NServiceBus;

    class DisableUsage
    {
        DisableUsage(Configure configure)
        {
            #region DisableImmediateDelayedRetriesConfigurationSourceUsage

            configure.CustomConfigurationSource(new DisableConfigurationSource());

            #endregion
        }
    }
}