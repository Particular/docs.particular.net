namespace Core8
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region ConfiguringNonDurable

            endpointConfiguration.UsePersistence<NonDurablePersistence, StorageType.Sagas>();
            endpointConfiguration.UsePersistence<NonDurablePersistence, StorageType.Subscriptions>();
            endpointConfiguration.UsePersistence<NonDurablePersistence, StorageType.Timeouts>();
            endpointConfiguration.UsePersistence<NonDurablePersistence, StorageType.Outbox>();
            
            #endregion
            
        }
    }
}
