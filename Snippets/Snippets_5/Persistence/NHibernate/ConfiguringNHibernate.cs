using NServiceBus;
using NServiceBus.Persistence;

class ConfiguringNHibernate
{
    public void Version_5_0()
    {
#pragma warning disable 618

        #region ConfiguringNHibernate 5.0

        BusConfiguration busConfiguration = new BusConfiguration();

        busConfiguration.UsePersistence<NHibernatePersistence>()
            .For(
                Storage.Sagas,
                Storage.Subscriptions,
                Storage.Timeouts,
                Storage.Outbox,
                Storage.GatewayDeduplication);

        #endregion
#pragma warning restore 618

    }

    public void Version_5_2()
    {
        #region ConfiguringNHibernate 5.2

        BusConfiguration busConfiguration = new BusConfiguration();

        busConfiguration.UsePersistence<NHibernatePersistence, StorageType.Sagas>();
        busConfiguration.UsePersistence<NHibernatePersistence, StorageType.Subscriptions>();
        busConfiguration.UsePersistence<NHibernatePersistence, StorageType.Timeouts>();
        busConfiguration.UsePersistence<NHibernatePersistence, StorageType.Outbox>();
        busConfiguration.UsePersistence<NHibernatePersistence, StorageType.GatewayDeduplication>();

        #endregion
    }
}