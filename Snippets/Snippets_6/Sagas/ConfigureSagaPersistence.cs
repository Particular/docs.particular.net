namespace Snippets6.Sagas
{
    using NServiceBus;
    using NServiceBus.Persistence;

    public class ConfigureSagaPersistence
    {

        public async void Simple()
        {
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();

            #region saga-configure

            endpointConfiguration.UsePersistence<PersistenceToUseGoesHere>();

            #endregion
        }

        public class PersistenceToUseGoesHere : PersistenceDefinition
        {
        }
    }
}
