using System;
using global::NHibernate.Cfg;
using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.Persistence.NHibernate;

class Usage
{
    static void Simple(EndpointConfiguration endpointConfiguration)
    {
        #region ConfiguringNHibernate

        // Use NHibernate for all persistence concerns
        endpointConfiguration.UsePersistence<NHibernatePersistence>();

        // or select specific concerns
        endpointConfiguration.UsePersistence<NHibernatePersistence, StorageType.Sagas>();
        endpointConfiguration.UsePersistence<NHibernatePersistence, StorageType.Subscriptions>();
        endpointConfiguration.UsePersistence<NHibernatePersistence, StorageType.Outbox>();

        #endregion
    }

    static void ConnectionString(EndpointConfiguration endpointConfiguration)
    {
        #region ConnectionStringAPI

        var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.ConnectionString(@"Data Source=.\SqlExpress;Database=nservicebus");

        #endregion
    }

    static void NHibernateSubscriptionCaching(EndpointConfiguration endpointConfiguration)
    {
        #region NHibernateSubscriptionCaching

        var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence, StorageType.Subscriptions>();
        persistence.EnableCachingForSubscriptionStorage(TimeSpan.FromMinutes(1));

        #endregion
    }


    static void CustomCommonConfiguration(EndpointConfiguration endpointConfiguration)
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

    static void SpecificNHibernateConfiguration(EndpointConfiguration endpointConfiguration)
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

        #endregion
    }


    static void CustomCommonConfigurationWarning(EndpointConfiguration endpointConfiguration)
    {
        #region CustomCommonNhibernateConfigurationWarning

        var nhConfiguration = new Configuration
        {
            Properties =
            {
                ["dialect"] = "NHibernate.Dialect.MsSql2008Dialect"
            }
        };

        var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence, StorageType.Subscriptions>();
        persistence.UseConfiguration(nhConfiguration);

        #endregion
    }
}