using NHibernate.Cfg;
using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.Persistence.NHibernate;

class ConfiguringNHibernate
{
    public void Version_5_0()
    {
#pragma warning disable 618

        #region ConfiguringNHibernate 5.0

        BusConfiguration busConfiguration = new BusConfiguration();

        //Use NHibernate for all persistence concerns
        busConfiguration.UsePersistence<NHibernatePersistence>();

        //or select specific concerns
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

        //Use NHibernate for all persistence concerns
        busConfiguration.UsePersistence<NHibernatePersistence>();

        //or select specific concerns
        busConfiguration.UsePersistence<NHibernatePersistence, StorageType.Sagas>();
        busConfiguration.UsePersistence<NHibernatePersistence, StorageType.Subscriptions>();
        busConfiguration.UsePersistence<NHibernatePersistence, StorageType.Timeouts>();
        busConfiguration.UsePersistence<NHibernatePersistence, StorageType.Outbox>();
        busConfiguration.UsePersistence<NHibernatePersistence, StorageType.GatewayDeduplication>();

        #endregion
    }


    public void CustomCommonConfiguration()
    {
        #region CustomCommonConfiguration

        BusConfiguration busConfiguration = new BusConfiguration();

        Configuration config = new Configuration();
        config.Properties["dialect"] = "NHibernate.Dialect.MsSql2008Dialect";
        config.Properties["connection.provider"] = "NHibernate.Connection.DriverConnectionProvider";
        config.Properties["connection.driver_class"] = "NHibernate.Driver.Sql2008ClientDriver";

        busConfiguration.UsePersistence<NHibernatePersistence>().UseConfiguration(config);

        #endregion
    }

    public void CustomSpecificConfiguration()
    {
        #region CustomSpecificConfiguration

        BusConfiguration busConfiguration = new BusConfiguration();

        Configuration config = new Configuration();
        config.Properties["dialect"] = "NHibernate.Dialect.MsSql2008Dialect";

        busConfiguration.UsePersistence<NHibernatePersistence>().UseSubscriptionStorageConfiguration(config);
        busConfiguration.UsePersistence<NHibernatePersistence>().UseGatewayDeduplicationConfiguration(config);
        busConfiguration.UsePersistence<NHibernatePersistence>().UseTimeoutStorageConfiguration(config);
        #endregion

    }

    public void CustomCommonConfigurationWarning()
    {
        #region CustomCommonConfigurationWarning

        BusConfiguration busConfiguration = new BusConfiguration();

        Configuration config = new Configuration();
        config.Properties["dialect"] = "NHibernate.Dialect.MsSql2008Dialect";

        busConfiguration.UsePersistence<NHibernatePersistence, StorageType.GatewayDeduplication>().UseConfiguration(config);
        #endregion
    }
}