namespace NHibernate_7
{
    using NServiceBus;
    using NServiceBus.Persistence;
    using NServiceBus.Persistence.NHibernate;

    class DisableSchemaUpdates
    {
        void DisableSchemaUpdate(EndpointConfiguration endpointConfiguration)
        {
            #region DisableSchemaUpdate

            endpointConfiguration.UsePersistence<NHibernatePersistence>()
                .DisableSchemaUpdate();

            #endregion
        }
        void DisableGatewaySchemaUpdate(EndpointConfiguration endpointConfiguration)
        {
            #region DisableGatewaySchemaUpdate

            endpointConfiguration.UsePersistence<NHibernatePersistence>()
                .DisableGatewayDeduplicationSchemaUpdate();

            #endregion
        }
        void DisableSubscriptionSchemaUpdate(EndpointConfiguration endpointConfiguration)
        {
            #region DisableSubscriptionSchemaUpdate

            endpointConfiguration.UsePersistence<NHibernatePersistence>()
                .DisableSubscriptionStorageSchemaUpdate();

            #endregion
        }
        void DisableTimeoutSchemaUpdate(EndpointConfiguration endpointConfiguration)
        {
            #region DisableTimeoutSchemaUpdate

            endpointConfiguration.UsePersistence<NHibernatePersistence>()
                .DisableTimeoutStorageSchemaUpdate();

            #endregion
        }
    }
}
