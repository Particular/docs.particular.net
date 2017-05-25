namespace Core3
{
    using NServiceBus;

    class Usage
    {
        Usage(Configure configure)
        {
            #region ConfiguringInMemory

            configure.InMemoryFaultManagement();
            configure.InMemorySagaPersister();
            configure.InMemorySubscriptionStorage();
            configure.RunGatewayWithInMemoryPersistence();
            configure.RunTimeoutManagerWithInMemoryPersistence();

            #endregion
        }
    }
}