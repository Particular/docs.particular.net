using NServiceBus;
using NServiceBus.Persistence;

class ConfiguringNHibernate
{
    public void Version_5_0()
    {
#pragma warning disable 618

        #region ConfiguringNHibernate 5.0

        var config = new BusConfiguration();

        config.UsePersistence<NHibernatePersistence>()
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

        var config = new BusConfiguration();

        config.UsePersistence<NHibernatePersistence, StorageType.Sagas>();
        config.UsePersistence<NHibernatePersistence, StorageType.Subscriptions>();
        config.UsePersistence<NHibernatePersistence, StorageType.Timeouts>();
        config.UsePersistence<NHibernatePersistence, StorageType.Outbox>();
        config.UsePersistence<NHibernatePersistence, StorageType.GatewayDeduplication>();

        #endregion
    }
}