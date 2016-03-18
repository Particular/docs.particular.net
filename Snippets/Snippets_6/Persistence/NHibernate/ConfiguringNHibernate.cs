namespace Snippets6.Persistence.NHibernate
{
    using System;
    using global::NHibernate.Cfg;
    using NServiceBus;
    using NServiceBus.Persistence;
    using NServiceBus.Persistence.NHibernate;

    class ConfiguringNHibernate
    {
        public void Version_5_2(EndpointConfiguration endpointConfiguration)
        {
            #region ConfiguringNHibernate

            //Use NHibernate for all persistence concerns
            endpointConfiguration.UsePersistence<NHibernatePersistence>();

            //or select specific concerns
            endpointConfiguration.UsePersistence<NHibernatePersistence, StorageType.Sagas>();
            endpointConfiguration.UsePersistence<NHibernatePersistence, StorageType.Subscriptions>();
            endpointConfiguration.UsePersistence<NHibernatePersistence, StorageType.Timeouts>();
            endpointConfiguration.UsePersistence<NHibernatePersistence, StorageType.Outbox>();
            endpointConfiguration.UsePersistence<NHibernatePersistence, StorageType.GatewayDeduplication>();

            #endregion

            #region NHibernateSubscriptionCaching

            endpointConfiguration.UsePersistence<NHibernatePersistence, StorageType.Subscriptions>()
                .EnableCachingForSubscriptionStorage(TimeSpan.FromMinutes(1));

            #endregion
        }


        public void CustomCommonConfiguration(EndpointConfiguration endpointConfiguration)
        {
            #region CommonNHibernateConfiguration

            Configuration nhConfiguration = new Configuration
            {
                Properties =
                {
                    ["dialect"] = "NHibernate.Dialect.MsSql2008Dialect",
                    ["connection.provider"] = "NHibernate.Connection.DriverConnectionProvider",
                    ["connection.driver_class"] = "NHibernate.Driver.Sql2008ClientDriver"
                }
            };

            endpointConfiguration.UsePersistence<NHibernatePersistence>()
                .UseConfiguration(nhConfiguration);

            #endregion
        }

        public void SpecificNHibernateConfiguration(EndpointConfiguration endpointConfiguration)
        {
            #region SpecificNHibernateConfiguration

            Configuration nhConfiguration = new Configuration
            {
                Properties =
                {
                    ["dialect"] = "NHibernate.Dialect.MsSql2008Dialect"
                }
            };

            endpointConfiguration.UsePersistence<NHibernatePersistence>()
                .UseSubscriptionStorageConfiguration(nhConfiguration);
            endpointConfiguration.UsePersistence<NHibernatePersistence>()
                .UseGatewayDeduplicationConfiguration(nhConfiguration);
            endpointConfiguration.UsePersistence<NHibernatePersistence>()
                .UseTimeoutStorageConfiguration(nhConfiguration);
            #endregion

        }
        

        public void CustomCommonConfigurationWarning(EndpointConfiguration endpointConfiguration)
        {
            #region CustomCommonNhibernateConfigurationWarning

            Configuration nhConfiguration = new Configuration
            {
                Properties =
                {
                    ["dialect"] = "NHibernate.Dialect.MsSql2008Dialect"
                }
            };

            endpointConfiguration.UsePersistence<NHibernatePersistence, StorageType.GatewayDeduplication>()
                .UseConfiguration(nhConfiguration);
            #endregion
        }
    }
}
