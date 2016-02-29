namespace Snippets6.Persistence.NHibernate
{
    using NServiceBus;
    using NServiceBus.Persistence;
    using NServiceBus.Persistence.NHibernate;

    class DisableSchemaUpdate
    {
        public void Usage()
        {

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();

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
