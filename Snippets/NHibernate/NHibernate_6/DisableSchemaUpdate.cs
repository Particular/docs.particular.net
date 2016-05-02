using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.Persistence.NHibernate;

class DisableSchemaUpdates
{
    void DisableSchemaUpdate(BusConfiguration busConfiguration)
    {
        #region DisableSchemaUpdate

        var persistence = busConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.DisableSchemaUpdate();

        #endregion
    }
    void DisableGatewaySchemaUpdate(BusConfiguration busConfiguration)
    {
        #region DisableGatewaySchemaUpdate

        var persistence = busConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.DisableGatewayDeduplicationSchemaUpdate();

        #endregion
    }

    void DisableSubscriptionSchemaUpdate(BusConfiguration busConfiguration)
    {
        #region DisableSubscriptionSchemaUpdate

        var persistence = busConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.DisableSubscriptionStorageSchemaUpdate();

        #endregion
    }

    void DisableTimeoutSchemaUpdate(BusConfiguration busConfiguration)
    {
        #region DisableTimeoutSchemaUpdate

        var persistence = busConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.DisableTimeoutStorageSchemaUpdate();

        #endregion
    }
}