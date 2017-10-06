using System;
using global::NHibernate.Cfg;
using NServiceBus;
using NServiceBus.Persistence.NHibernate;

class Usage
{
    void Simple(EndpointConfiguration endpointConfiguration)
    {
        #region ConfiguringNHibernate

        // Use NHibernate for all persistence concerns
        endpointConfiguration.UsePersistence<NHibernatePersistence>();

        // or select specific concerns
        endpointConfiguration.UsePersistence<NHibernatePersistence, StorageType.Sagas>();
        endpointConfiguration.UsePersistence<NHibernatePersistence, StorageType.Subscriptions>();
        endpointConfiguration.UsePersistence<NHibernatePersistence, StorageType.Timeouts>();
        endpointConfiguration.UsePersistence<NHibernatePersistence, StorageType.Outbox>();
        endpointConfiguration.UsePersistence<NHibernatePersistence, StorageType.GatewayDeduplication>();

        #endregion
    }

    void ConnectionString(EndpointConfiguration endpointConfiguration)
    {
        #region ConnectionStringAPI

        var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.ConnectionString(@"Data Source=.\SqlExpress;Database=nservicebus");

        #endregion
    }

    void NHibernateSubscriptionCaching(EndpointConfiguration endpointConfiguration)
    {
        #region NHibernateSubscriptionCaching

        var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence, StorageType.Subscriptions>();
        persistence.EnableCachingForSubscriptionStorage(TimeSpan.FromMinutes(1));

        #endregion
    }


    void CustomCommonConfiguration(EndpointConfiguration endpointConfiguration)
    {
        #region CommonNHibernateConfiguration
        var nhConfiguration = new Configuration
        {
            Properties =
            {
                ["dialect"] = "NHibernate.Dialect.MsSql2008Dialect",
                ["connection.provider"] = "NHibernate.Connection.DriverConnectionProvider",
                ["connection.driver_class"] = "NHibernate.Driver.Sql2008ClientDriver"
            }
        };

        var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.UseConfiguration(nhConfiguration);

        #endregion
    }

    void SpecificNHibernateConfiguration(EndpointConfiguration endpointConfiguration)
    {
        #region SpecificNHibernateConfiguration

        var nhConfiguration = new Configuration
        {
            Properties =
            {
                ["dialect"] = "NHibernate.Dialect.MsSql2008Dialect"
            }
        };

        var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.UseSubscriptionStorageConfiguration(nhConfiguration);
        persistence.UseGatewayDeduplicationConfiguration(nhConfiguration);
        persistence.UseTimeoutStorageConfiguration(nhConfiguration);

        #endregion
    }


    void CustomCommonConfigurationWarning(EndpointConfiguration endpointConfiguration)
    {
        #region CustomCommonNhibernateConfigurationWarning

        var nhConfiguration = new Configuration
        {
            Properties =
            {
                ["dialect"] = "NHibernate.Dialect.MsSql2008Dialect"
            }
        };

        var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence, StorageType.GatewayDeduplication>();
        persistence.UseConfiguration(nhConfiguration);

        #endregion
    }
}