namespace Snippets5.Sagas
{
    using NServiceBus;
    using NServiceBus.Persistence;

    public class ConfigureSagaPersistence
    {

        public void Simple()
        {
            #region saga-configure

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UsePersistence<RavenDBPersistence>(); //or NHibernatePersistence
            IStartableBus bus = Bus.Create(busConfiguration);

            #endregion
        }
    }
}
