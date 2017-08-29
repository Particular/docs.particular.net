namespace Raven_5
{
    using NServiceBus;

    class UnversionedSubscriptions
    {
        UnversionedSubscriptions(EndpointConfiguration endpointConfiguration)
        {
            #region DisableSubscriptionVersioning
            var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
            persistence.DisableSubscriptionVersioning();
            #endregion
        }
    }
}