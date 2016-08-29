using NServiceBus;

class DeadLetterQueueForwarding
{
    void ConfigureDeadLetterQueueForwarding(EndpointConfiguration endpointConfiguration)
    {
        #region asb-configure-dead-letter-queue-forwarding

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.Queues().ForwardDeadLetteredMessagesTo(queuename => queuename != "error" && queuename != "audit" && queuename != "centralizeddlq", "centralizeddlq");
        transport.Subscriptions().ForwardDeadLetteredMessagesTo("centralizeddlq");

        #endregion
    }
}