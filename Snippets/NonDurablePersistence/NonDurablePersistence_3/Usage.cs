namespace NonDurablePersistence_3
{
    using NServiceBus;

    class Usage
    {
        public Usage(EndpointConfiguration endpointConfiguration)
        {
            #region ConfiguringNonDurable

            endpointConfiguration.UsePersistence<NonDurablePersistence, StorageType.Sagas>();
            endpointConfiguration.UsePersistence<NonDurablePersistence, StorageType.Subscriptions>();
            endpointConfiguration.UsePersistence<NonDurablePersistence, StorageType.Outbox>();

            #endregion
        }
    }
}
