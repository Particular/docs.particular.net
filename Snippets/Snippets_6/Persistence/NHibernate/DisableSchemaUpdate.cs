namespace Snippets6.Persistence.NHibernate
{
    using NServiceBus;
    using NServiceBus.Persistence;
    using NServiceBus.Persistence.NHibernate;

    class DisableSchemaUpdate
    {
        public void Usage()
        {

            BusConfiguration busConfiguration = new BusConfiguration();

            #region DisableSchemaUpdate

            busConfiguration.UsePersistence<NHibernatePersistence>()
                .DisableSchemaUpdate();

            #endregion
            #region DisableGatewaySchemaUpdate

            busConfiguration.UsePersistence<NHibernatePersistence>()
                .DisableGatewayDeduplicationSchemaUpdate();

            #endregion
            #region DisableSubscriptionSchemaUpdate

            busConfiguration.UsePersistence<NHibernatePersistence>()
                .DisableSubscriptionStorageSchemaUpdate();

            #endregion
            #region DisableTimeoutSchemaUpdate

            busConfiguration.UsePersistence<NHibernatePersistence>()
                .DisableTimeoutStorageSchemaUpdate();

            #endregion
        }
    }
}
