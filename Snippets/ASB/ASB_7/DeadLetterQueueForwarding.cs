using NServiceBus;

class DeadLetterQueueForwarding
{
    void ConfigureDeadLetterQueueForwarding(EndpointConfiguration endpointConfiguration)
    {
        #region asb-configure-dead-letter-queue-forwarding

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var queues = transport.Queues();
        queues.ForwardDeadLetteredMessagesTo(
                queuename =>
                {
                    return queuename != "error" &&
                           queuename != "audit" &&
                           queuename != "centralizeddlq";
                },
                "centralizeddlq")
            ;
        var subscriptions = transport.Subscriptions();
        subscriptions.ForwardDeadLetteredMessagesTo("centralizeddlq");

        #endregion
    }
}