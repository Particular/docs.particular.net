
using NServiceBus;

class ServiceControlRemoteQueues
{
    ServiceControlRemoteQueues(EndpointConfiguration endpointConfiguration)
    {
        #region ConfigMsmqErrorWithCode

        endpointConfiguration.SendFailedMessagesTo("targetErrorQueue@machinename");

        #endregion
    }
}