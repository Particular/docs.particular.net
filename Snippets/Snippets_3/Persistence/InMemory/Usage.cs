namespace Snippets3.Persistence.InMemory
{
    using NServiceBus;

    class Usage
    {
        public Usage()
        {
            #region ConfiguringInMemory

            Configure configure = Configure.With();
            configure.InMemoryFaultManagement();
            configure.InMemorySagaPersister();
            configure.InMemorySubscriptionStorage();
            configure.RunGatewayWithInMemoryPersistence();
            configure.RunTimeoutManagerWithInMemoryPersistence();

            #endregion
        }
    }
}