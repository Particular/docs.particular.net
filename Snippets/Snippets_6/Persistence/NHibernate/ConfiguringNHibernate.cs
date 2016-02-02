namespace Snippets6.Persistence.NHibernate
{
    using System;
    using global::NHibernate.Cfg;
    using NServiceBus;
    using NServiceBus.Persistence;
    using NServiceBus.Persistence.NHibernate;

    class ConfiguringNHibernate
    {
        public void Version_5_2()
        {
            #region ConfiguringNHibernate

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

            #region NHibernateSubscriptionCaching

            busConfiguration.UsePersistence<NHibernatePersistence, StorageType.Subscriptions>()
                .EnableCachingForSubscriptionStorage(TimeSpan.FromMinutes(1));

            #endregion
        }


        public void CustomCommonConfiguration()
        {
            #region CommonNHibernateConfiguration

            BusConfiguration busConfiguration = new BusConfiguration();

            Configuration nhConfiguration = new Configuration
            {
                Properties =
                {
                    ["dialect"] = "NHibernate.Dialect.MsSql2008Dialect",
                    ["connection.provider"] = "NHibernate.Connection.DriverConnectionProvider",
                    ["connection.driver_class"] = "NHibernate.Driver.Sql2008ClientDriver"
                }
            };

            busConfiguration.UsePersistence<NHibernatePersistence>()
                .UseConfiguration(nhConfiguration);

            #endregion
        }

        public void SpecificNHibernateConfiguration()
        {
            #region SpecificNHibernateConfiguration

            BusConfiguration busConfiguration = new BusConfiguration();

            Configuration nhConfiguration = new Configuration
            {
                Properties =
                {
                    ["dialect"] = "NHibernate.Dialect.MsSql2008Dialect"
                }
            };

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

            Configuration nhConfiguration = new Configuration
            {
                Properties =
                {
                    ["dialect"] = "NHibernate.Dialect.MsSql2008Dialect"
                }
            };

            busConfiguration.UsePersistence<NHibernatePersistence, StorageType.GatewayDeduplication>()
                .UseConfiguration(nhConfiguration);
            #endregion
        }
    }
}
