namespace Core6.Persistence.Msmq
{
    using NServiceBus;

    class Usage
    {
        void Configuration(EndpointConfiguration endpointConfiguration)
        {
            #region ConfiguringMsmqPersistence

            endpointConfiguration.UsePersistence<MsmqPersistence>();

            #endregion
        }

        void WithOtherPersisters(EndpointConfiguration endpointConfiguration)
        {
            #region MsmqPersistenceWithOtherPersisters

            endpointConfiguration.UsePersistence<NonDurablePersistence, StorageType.Sagas>();
            endpointConfiguration.UsePersistence<MsmqPersistence, StorageType.Subscriptions>();

            #endregion
        }

        void OverrideSubscriptionQueue(EndpointConfiguration endpointConfiguration)
        {
            #region MsmqSubscriptionCode

            var persistence = endpointConfiguration.UsePersistence<MsmqPersistence>();
            persistence.SubscriptionQueue("YourEndpointName.Subscriptions");

            #endregion
        }
    }
}