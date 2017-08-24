namespace Raven_4
{
    using NServiceBus;

    class UnversionedSubscriptions
    {
        UnversionedSubscriptions(EndpointConfiguration endpointConfiguration)
        {
            #region DisableSubscriptionVersioning 4.2
            var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
            persistence.DisableSubscriptionVersioning();
            #endregion
        }
    }
}