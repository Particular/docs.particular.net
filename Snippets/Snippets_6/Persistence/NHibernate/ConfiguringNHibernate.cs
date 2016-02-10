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

            EndpointConfiguration configuration = new EndpointConfiguration();

            //Use NHibernate for all persistence concerns
            configuration.UsePersistence<NHibernatePersistence>();

            //or select specific concerns
            configuration.UsePersistence<NHibernatePersistence, StorageType.Sagas>();
            configuration.UsePersistence<NHibernatePersistence, StorageType.Subscriptions>();
            configuration.UsePersistence<NHibernatePersistence, StorageType.Timeouts>();
            configuration.UsePersistence<NHibernatePersistence, StorageType.Outbox>();
            configuration.UsePersistence<NHibernatePersistence, StorageType.GatewayDeduplication>();

            #endregion

            #region NHibernateSubscriptionCaching

            configuration.UsePersistence<NHibernatePersistence, StorageType.Subscriptions>()
                .EnableCachingForSubscriptionStorage(TimeSpan.FromMinutes(1));

            #endregion
        }


        public void CustomCommonConfiguration()
        {
            #region CommonNHibernateConfiguration

            EndpointConfiguration configuration = new EndpointConfiguration();

            Configuration nhConfiguration = new Configuration
            {
                Properties =
                {
                    ["dialect"] = "NHibernate.Dialect.MsSql2008Dialect",
                    ["connection.provider"] = "NHibernate.Connection.DriverConnectionProvider",
                    ["connection.driver_class"] = "NHibernate.Driver.Sql2008ClientDriver"
                }
            };

            configuration.UsePersistence<NHibernatePersistence>()
                .UseConfiguration(nhConfiguration);

            #endregion
        }

        public void SpecificNHibernateConfiguration()
        {
            #region SpecificNHibernateConfiguration

            EndpointConfiguration configuration = new EndpointConfiguration();

            Configuration nhConfiguration = new Configuration
            {
                Properties =
                {
                    ["dialect"] = "NHibernate.Dialect.MsSql2008Dialect"
                }
            };

            configuration.UsePersistence<NHibernatePersistence>()
                .UseSubscriptionStorageConfiguration(nhConfiguration);
            configuration.UsePersistence<NHibernatePersistence>()
                .UseGatewayDeduplicationConfiguration(nhConfiguration);
            configuration.UsePersistence<NHibernatePersistence>()
                .UseTimeoutStorageConfiguration(nhConfiguration);
            #endregion

        }
        

        public void CustomCommonConfigurationWarning()
        {
            #region CustomCommonNhibernateConfigurationWarning

            EndpointConfiguration configuration = new EndpointConfiguration();

            Configuration nhConfiguration = new Configuration
            {
                Properties =
                {
                    ["dialect"] = "NHibernate.Dialect.MsSql2008Dialect"
                }
            };

            configuration.UsePersistence<NHibernatePersistence, StorageType.GatewayDeduplication>()
                .UseConfiguration(nhConfiguration);
            #endregion
        }
    }
}
