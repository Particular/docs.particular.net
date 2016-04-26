namespace NHibernate_6
{
    using NServiceBus;
    using NServiceBus.Persistence;
    using NServiceBus.Persistence.NHibernate;

    class DisableSchemaUpdates
    {
        void DisableSchemaUpdate(BusConfiguration busConfiguration)
        {
            #region DisableSchemaUpdate

            busConfiguration.UsePersistence<NHibernatePersistence>()
                .DisableSchemaUpdate();

            #endregion
        }
        void DisableGatewaySchemaUpdate(BusConfiguration busConfiguration)
        {
            #region DisableGatewaySchemaUpdate

            busConfiguration.UsePersistence<NHibernatePersistence>()
                .DisableGatewayDeduplicationSchemaUpdate();

            #endregion
        }
        void DisableSubscriptionSchemaUpdate(BusConfiguration busConfiguration)
        {
            #region DisableSubscriptionSchemaUpdate

            busConfiguration.UsePersistence<NHibernatePersistence>()
                .DisableSubscriptionStorageSchemaUpdate();

            #endregion
        }
        void DisableTimeoutSchemaUpdate(BusConfiguration busConfiguration)
        {
            #region DisableTimeoutSchemaUpdate

            busConfiguration.UsePersistence<NHibernatePersistence>()
                .DisableTimeoutStorageSchemaUpdate();

            #endregion
        }
    }
}
