namespace NonDurablePersistence_2
{
    using NServiceBus;

    class Usage
    {
        public Usage(EndpointConfiguration endpointConfiguration)
        {
            #region ConfiguringNonDurable

            // Use NonDurable persistence for all concerns
            endpointConfiguration.UsePersistence<NonDurablePersistence>();

            // or select specific concerns
            endpointConfiguration.UsePersistence<NonDurablePersistence, StorageType.Sagas>();
            endpointConfiguration.UsePersistence<NonDurablePersistence, StorageType.Subscriptions>();
            endpointConfiguration.UsePersistence<NonDurablePersistence, StorageType.Outbox>();

            #endregion
        }
    }
}
