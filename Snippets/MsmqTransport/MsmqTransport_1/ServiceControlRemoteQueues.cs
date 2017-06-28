
using NServiceBus;

class ServiceControlRemoteQueues
{
    ServiceControlRemoteQueues(EndpointConfiguration endpointConfiguration)
    {
        # region ConfigMsmqErrorWithCode

        endpointConfiguration.SendFailedMessagesTo("targetErrorQueue@machinename");

        # endregion
    }
}

class CustomConfigurationSource
{
    CustomConfigurationSource(EndpointConfiguration endpointConfiguration)
    {
        #region UseCustomConfigurationSourceForErrorQueueRemoateMachineConfig

        endpointConfiguration.CustomConfigurationSource(new ConfigurationSource());

        #endregion
    }
}