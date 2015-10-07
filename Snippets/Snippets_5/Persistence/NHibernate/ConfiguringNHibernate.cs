namespace Snippets5.Persistence.NHibernate
{
    using global::NHibernate.Cfg;
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


            #region NHibernateSubscriptionCaching 5.0
            
            busConfiguration.UsePersistence<NHibernatePersistence>()
                .EnableCachingForSubscriptionStorage(System.TimeSpan.FromSeconds(10));
            
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

            #region NHibernateSubscriptionCaching 5.2

            busConfiguration.UsePersistence<NHibernatePersistence, StorageType.Subscriptions>()
                .EnableCachingForSubscriptionStorage(System.TimeSpan.FromMinutes(1));

            #endregion
        }


        public void CustomCommonConfiguration()
        {
            #region CommonNHibernateConfiguration

            BusConfiguration busConfiguration = new BusConfiguration();

            Configuration nhConfiguration = new Configuration();
            nhConfiguration.Properties["dialect"] = "NHibernate.Dialect.MsSql2008Dialect";
            nhConfiguration.Properties["connection.provider"] = "NHibernate.Connection.DriverConnectionProvider";
            nhConfiguration.Properties["connection.driver_class"] = "NHibernate.Driver.Sql2008ClientDriver";

            busConfiguration.UsePersistence<NHibernatePersistence>()
                .UseConfiguration(nhConfiguration);

            #endregion
        }

        public void SpecificNHibernateConfiguration()
        {
            #region SpecificNHibernateConfiguration

            BusConfiguration busConfiguration = new BusConfiguration();

            Configuration nhConfiguration = new Configuration();
            nhConfiguration.Properties["dialect"] = "NHibernate.Dialect.MsSql2008Dialect";

            busConfiguration.UsePersistence<NHibernatePersistence>()
                .UseSubscriptionStorageConfiguration(nhConfiguration);
            busConfiguration.UsePersistence<NHibernatePersistence>()
                .UseGatewayDeduplicationConfiguration(nhConfiguration);
            busConfiguration.UsePersistence<NHibernatePersistence>()
                .UseTimeoutStorageConfiguration(nhConfiguration);
            #endregion

        }

        public void CustomCommonConfigurationWarning()
        {
            #region CustomCommonNhibernateConfigurationWarning

            BusConfiguration busConfiguration = new BusConfiguration();

            Configuration nhConfiguration = new Configuration();
            nhConfiguration.Properties["dialect"] = "NHibernate.Dialect.MsSql2008Dialect";

            busConfiguration.UsePersistence<NHibernatePersistence, StorageType.GatewayDeduplication>()
                .UseConfiguration(nhConfiguration);
            #endregion
        }
    }
}
