using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.Persistence.NHibernate;

class DisableSchemaUpdates
{
    void DisableSchemaUpdate(EndpointConfiguration endpointConfiguration)
    {
        #region DisableSchemaUpdate

        var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.DisableSchemaUpdate();

        #endregion
    }

    void DisableSubscriptionSchemaUpdate(EndpointConfiguration endpointConfiguration)
    {
        #region DisableSubscriptionSchemaUpdate

        var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.DisableSubscriptionStorageSchemaUpdate();

        #endregion
    }
}