namespace Snippets5.Sagas
{
    using NServiceBus;
    using NServiceBus.Persistence;

    class ConfigureSagaPersistence
    {
        ConfigureSagaPersistence(BusConfiguration busConfiguration)
        {
            #region saga-configure

            busConfiguration.UsePersistence<PersistenceToUseGoesHere>();

            #endregion
        }

        public class PersistenceToUseGoesHere: PersistenceDefinition
        {
        }
    }
}
