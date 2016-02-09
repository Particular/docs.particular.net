namespace Snippets5.Sagas
{
    using NServiceBus;
    using NServiceBus.Persistence;

    public class ConfigureSagaPersistence
    {

        public async void Simple()
        {
            EndpointConfiguration configuration = new EndpointConfiguration();

            #region saga-configure

            configuration.UsePersistence<PersistenceToUseGoesHere>();

            #endregion
        }

        public class PersistenceToUseGoesHere : PersistenceDefinition
        {
        }
    }
}
