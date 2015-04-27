using NServiceBus;

class ConfiguringInMemory
{
    public void Foo()
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