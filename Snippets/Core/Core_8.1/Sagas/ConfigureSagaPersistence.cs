namespace Core8.Sagas
{
    using NServiceBus;
    using NServiceBus.Persistence;

    class ConfigureSagaPersistence
    {

        void Simple(EndpointConfiguration endpointConfiguration)
        {
            #region saga-configure

            endpointConfiguration.UsePersistence<PersistenceToUseGoesHere>();

            #endregion
        }

        public class PersistenceToUseGoesHere :
            PersistenceDefinition
        {
        }
    }
}
