namespace Snippets6.Persistence.NHibernate
{
    using NServiceBus;
    using NServiceBus.Persistence;
    using NServiceBus.Persistence.NHibernate;

    class DisableSchemaUpdate
    {
        public DisableSchemaUpdate(EndpointConfiguration endpointConfiguration)
        {
            #region DisableSchemaUpdate

            endpointConfiguration.UsePersistence<NHibernatePersistence>()
                .DisableSchemaUpdate();

            #endregion
            #region DisableGatewaySchemaUpdate

            endpointConfiguration.UsePersistence<NHibernatePersistence>()
                .DisableGatewayDeduplicationSchemaUpdate();

            #endregion
            #region DisableSubscriptionSchemaUpdate

            endpointConfiguration.UsePersistence<NHibernatePersistence>()
                .DisableSubscriptionStorageSchemaUpdate();

            #endregion
            #region DisableTimeoutSchemaUpdate

            endpointConfiguration.UsePersistence<NHibernatePersistence>()
                .DisableTimeoutStorageSchemaUpdate();

            #endregion
        }
    }
}
