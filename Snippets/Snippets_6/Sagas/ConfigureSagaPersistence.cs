namespace Snippets5.Sagas
{
    using NServiceBus;
    using NServiceBus.Persistence;

    public class ConfigureSagaPersistence
    {

        public async void Simple()
        {
            BusConfiguration busConfiguration = new BusConfiguration();

            #region saga-configure

            busConfiguration.UsePersistence<PersistenceToUseGoesHere>();

            #endregion
        }

        public class PersistenceToUseGoesHere : PersistenceDefinition
        {
        }
    }
}
