namespace Snippets4.Persistence.InMemory
{
    using NServiceBus;

    class Usage
    {
        public Usage()
        {
            #region ConfiguringInMemory

            Configure.With()
                .DefaultBuilder()
                .InMemoryFaultManagement()
                .InMemorySagaPersister()
                .InMemorySubscriptionStorage()
                .RunGatewayWithInMemoryPersistence()
                .UseInMemoryTimeoutPersister();

            #endregion
        }
    }
}