using NServiceBus;

class DisableDeadLetterQueueing
{
    DisableDeadLetterQueueing(EndpointConfiguration endpointConfiguration)
    {
        #region disable-dlq

        var transport = new MsmqTransport
        {
            UseDeadLetterQueue = false
        };
        endpointConfiguration.UseTransport(transport);

        #endregion
    }
}
