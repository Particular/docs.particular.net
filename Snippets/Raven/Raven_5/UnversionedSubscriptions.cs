namespace Raven_5
{
    using NServiceBus;

    class UnversionedSubscriptions
    {
        void UseUnversionedSubscriptions(EndpointConfiguration endpointConfiguration)
        {
            #region DisableSubscriptionVersioning
            var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
            persistence.DisableSubscriptionVersioning();
            #endregion
        }

#pragma warning disable CS0618 // Type or member is obsolete

        void LegacySubscriptionVersioning(EndpointConfiguration endpointConfiguration)
        {
            #region LegacySubscriptionVersioning
            var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
            persistence.UseLegacyVersionedSubscriptions();
            #endregion
        }

#pragma warning restore CS0618 // Type or member is obsolete
    }
}