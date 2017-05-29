namespace ServiceControl.ConfigureEndpoints
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region UseCustomConfigurationSourceForErrorQueueRemoateMachineConfig

            endpointConfiguration.CustomConfigurationSource(new ConfigurationSource());

            #endregion
        }
    }
}