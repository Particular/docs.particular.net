using System;
using global::NHibernate.Cfg;
using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.Persistence.NHibernate;

class ConfiguringNHibernate
{
    void Version_5_0(BusConfiguration busConfiguration)
    {
#pragma warning disable 618

        #region ConfiguringNHibernate 5.0

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


        #region NHibernateSubscriptionCaching 5.0

        var persistence = busConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.EnableCachingForSubscriptionStorage(TimeSpan.FromSeconds(10));

        #endregion
#pragma warning restore 618
    }

    void Version_5_2(BusConfiguration busConfiguration)
    {
        #region ConfiguringNHibernate 5.2

        //Use NHibernate for all persistence concerns
        busConfiguration.UsePersistence<NHibernatePersistence>();

        //or select specific concerns
        busConfiguration.UsePersistence<NHibernatePersistence, StorageType.Sagas>();
        busConfiguration.UsePersistence<NHibernatePersistence, StorageType.Subscriptions>();
        busConfiguration.UsePersistence<NHibernatePersistence, StorageType.Timeouts>();
        busConfiguration.UsePersistence<NHibernatePersistence, StorageType.Outbox>();
        busConfiguration.UsePersistence<NHibernatePersistence, StorageType.GatewayDeduplication>();

        #endregion

        #region NHibernateSubscriptionCaching 5.2

        var persistence = busConfiguration.UsePersistence<NHibernatePersistence, StorageType.Subscriptions>();
        persistence.EnableCachingForSubscriptionStorage(TimeSpan.FromMinutes(1));

        #endregion
    }


    void CustomCommonConfiguration(BusConfiguration busConfiguration)
    {
        #region CommonNHibernateConfiguration

        Configuration nhConfiguration = new Configuration();
        nhConfiguration.Properties["dialect"] = "NHibernate.Dialect.MsSql2008Dialect";
        nhConfiguration.Properties["connection.provider"] = "NHibernate.Connection.DriverConnectionProvider";
        nhConfiguration.Properties["connection.driver_class"] = "NHibernate.Driver.Sql2008ClientDriver";

        var persistence = busConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.UseConfiguration(nhConfiguration);

        #endregion
    }

    void SpecificNHibernateConfiguration(BusConfiguration busConfiguration)
    {
        #region SpecificNHibernateConfiguration

        Configuration nhConfiguration = new Configuration();
        nhConfiguration.Properties["dialect"] = "NHibernate.Dialect.MsSql2008Dialect";

        var persistence = busConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.UseSubscriptionStorageConfiguration(nhConfiguration);
        persistence.UseGatewayDeduplicationConfiguration(nhConfiguration);
        persistence.UseTimeoutStorageConfiguration(nhConfiguration);
        #endregion

    }


    void CustomCommonConfigurationWarning(BusConfiguration busConfiguration)
    {
        #region CustomCommonNhibernateConfigurationWarning

        Configuration nhConfiguration = new Configuration();
        nhConfiguration.Properties["dialect"] = "NHibernate.Dialect.MsSql2008Dialect";

        var persistence = busConfiguration.UsePersistence<NHibernatePersistence, StorageType.GatewayDeduplication>();
        persistence.UseConfiguration(nhConfiguration);
        #endregion
    }
}