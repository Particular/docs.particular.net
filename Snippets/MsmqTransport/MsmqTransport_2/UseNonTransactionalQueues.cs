using NServiceBus;

class UseNonTransactionalQueues
{
    UseNonTransactionalQueues(EndpointConfiguration endpointConfiguration)
    {
        #region use-nontransactional-queues

        var transport = new MsmqTransport
        {
            UseTransactionalQueues = false
        };
        endpointConfiguration.UseTransport(transport);

        #endregion
    }
}
