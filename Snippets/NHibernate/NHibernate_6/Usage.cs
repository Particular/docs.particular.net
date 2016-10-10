using System;
using global::NHibernate.Cfg;
using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.Persistence.NHibernate;

class Usage
{
    Usage(BusConfiguration busConfiguration)
    {
        #region ConfiguringNHibernate

        // Use NHibernate for all persistence concerns
        busConfiguration.UsePersistence<NHibernatePersistence>();

        // or select specific concerns
        busConfiguration.UsePersistence<NHibernatePersistence, StorageType.Sagas>();
        busConfiguration.UsePersistence<NHibernatePersistence, StorageType.Subscriptions>();
        busConfiguration.UsePersistence<NHibernatePersistence, StorageType.Timeouts>();
        busConfiguration.UsePersistence<NHibernatePersistence, StorageType.Outbox>();
        busConfiguration.UsePersistence<NHibernatePersistence, StorageType.GatewayDeduplication>();

        #endregion

        #region NHibernateSubscriptionCaching

        var persistence = busConfiguration.UsePersistence<NHibernatePersistence, StorageType.Subscriptions>();
        persistence.EnableCachingForSubscriptionStorage(TimeSpan.FromMinutes(1));

        #endregion
    }

    void ConnectionString(BusConfiguration busConfiguration)
    {
        #region ConnectionStringAPI

        var persistence = busConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.ConnectionString(@"Data Source=.\SqlExpress;Database=nservicebus");

        #endregion
    }


    void CustomCommonConfiguration(BusConfiguration busConfiguration)
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

        var persistence = busConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.UseConfiguration(nhConfiguration);

        #endregion
    }

    void SpecificNHibernateConfiguration(BusConfiguration busConfiguration)
    {
        #region SpecificNHibernateConfiguration

        var nhConfiguration = new Configuration
        {
            Properties =
            {
                ["dialect"] = "NHibernate.Dialect.MsSql2008Dialect"
            }
        };

        var persistence = busConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.UseSubscriptionStorageConfiguration(nhConfiguration);
        persistence.UseGatewayDeduplicationConfiguration(nhConfiguration);
        persistence.UseTimeoutStorageConfiguration(nhConfiguration);
        #endregion

    }


    void CustomCommonConfigurationWarning(BusConfiguration busConfiguration)
    {
        #region CustomCommonNhibernateConfigurationWarning

        var nhConfiguration = new Configuration
        {
            Properties =
            {
                ["dialect"] = "NHibernate.Dialect.MsSql2008Dialect"
            }
        };

        var persistence = busConfiguration.UsePersistence<NHibernatePersistence, StorageType.GatewayDeduplication>();
        persistence.UseConfiguration(nhConfiguration);
        #endregion
    }
}