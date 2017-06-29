
using NServiceBus;

class ServiceControlRemoteQueues
{
    ServiceControlRemoteQueues(EndpointConfiguration endpointConfiguration)
    {
        # region ConfigMsmqErrorWithCode1

        endpointConfiguration.SendFailedMessagesTo("targetErrorQueue@machinename");

        # endregion
    }
}