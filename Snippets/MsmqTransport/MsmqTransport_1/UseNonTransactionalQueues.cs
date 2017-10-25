using NServiceBus;

class UseNonTransactionalQueues
{
    UseNonTransactionalQueues(EndpointConfiguration endpointConfiguration)
    {
        #region use-nontransactional-queues

        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        transport.UseNonTransactionalQueues();

        #endregion
    }
}
