using NServiceBus;

class DisableDeadLetterQueueing
{
    DisableDeadLetterQueueing(EndpointConfiguration endpointConfiguration)
    {
        #region disable-dlq

        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        transport.DisableDeadLetterQueueing();

        #endregion
    }
}
