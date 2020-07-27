namespace Raven_6
{
    using NServiceBus;

    class SagasConfiguration
    {
        SagasConfiguration(EndpointConfiguration endpointConfiguration)
        {
            #region sagas-pessimistic-lock
            var sagasConfig = endpointConfiguration.UsePersistence<RavenDBPersistence>()
                .Sagas();
            sagasConfig.UsePessimisticLocking();
            #endregion
        }
    }
}