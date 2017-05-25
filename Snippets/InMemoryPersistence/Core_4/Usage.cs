namespace Core4
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
            configure.UseInMemoryTimeoutPersister();

            #endregion
        }
    }
}