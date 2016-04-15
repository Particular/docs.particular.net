namespace Core6.Sagas
{
    using NServiceBus;
    using NServiceBus.Persistence;

    class ConfigureSagaPersistence
    {

        async void Simple(EndpointConfiguration endpointConfiguration)
        {
            #region saga-configure

            endpointConfiguration.UsePersistence<PersistenceToUseGoesHere>();

            #endregion
        }

        public class PersistenceToUseGoesHere : PersistenceDefinition
        {
        }
    }
}
