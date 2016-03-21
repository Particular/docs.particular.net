namespace Snippets5.Persistence.NHibernate
{
    using NServiceBus;
    using NServiceBus.Persistence;
    using NServiceBus.Persistence.NHibernate;

    class DisableSchemaUpdate
    {
        DisableSchemaUpdate(BusConfiguration busConfiguration)
        {
            #region DisableSchemaUpdate 5.0

            busConfiguration.UsePersistence<NHibernatePersistence>()
                .DisableSchemaUpdate();

            #endregion
            #region DisableGatewaySchemaUpdate 5.0

            busConfiguration.UsePersistence<NHibernatePersistence>()
                .DisableGatewayDeduplicationSchemaUpdate();

            #endregion
            #region DisableSubscriptionSchemaUpdate 5.0

            busConfiguration.UsePersistence<NHibernatePersistence>()
                .DisableSubscriptionStorageSchemaUpdate();

            #endregion
            #region DisableTimeoutSchemaUpdate 5.0

            busConfiguration.UsePersistence<NHibernatePersistence>()
                .DisableTimeoutStorageSchemaUpdate();

            #endregion
        }
    }
}
