using NServiceBus;

class UnversionedSubscriptions
{
    void UseUnversionedSubscriptions(EndpointConfiguration endpointConfiguration)
    {
        #region 4to5-DisableSubscriptionVersioning
        var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.DisableSubscriptionVersioning();
        #endregion
    }

#pragma warning disable CS0618 // Type or member is obsolete

    void LegacySubscriptionVersioning(EndpointConfiguration endpointConfiguration)
    {
        #region 4to5-LegacySubscriptionVersioning
        var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.UseLegacyVersionedSubscriptions();
        #endregion
    }

#pragma warning restore CS0618 // Type or member is obsolete
}