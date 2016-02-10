namespace Snippets6.Persistence.NHibernate
{
    using NServiceBus;
    using NServiceBus.Persistence;
    using NServiceBus.Persistence.NHibernate;

    class DisableSchemaUpdate
    {
        public void Usage()
        {

            EndpointConfiguration configuration = new EndpointConfiguration();

            #region DisableSchemaUpdate

            configuration.UsePersistence<NHibernatePersistence>()
                .DisableSchemaUpdate();

            #endregion
            #region DisableGatewaySchemaUpdate

            configuration.UsePersistence<NHibernatePersistence>()
                .DisableGatewayDeduplicationSchemaUpdate();

            #endregion
            #region DisableSubscriptionSchemaUpdate

            configuration.UsePersistence<NHibernatePersistence>()
                .DisableSubscriptionStorageSchemaUpdate();

            #endregion
            #region DisableTimeoutSchemaUpdate

            configuration.UsePersistence<NHibernatePersistence>()
                .DisableTimeoutStorageSchemaUpdate();

            #endregion
        }
    }
}
