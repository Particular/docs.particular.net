namespace ServiceControl.ConfigureEndpoints
{
    using NServiceBus;

    class ConfigErrorQueue
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region ConfigMsmqErrorWithCode

            endpointConfiguration.SendFailedMessagesTo("targetErrorQueue@machinename");

            #ConfigMsmqErrorWithCode
        }
    }
}