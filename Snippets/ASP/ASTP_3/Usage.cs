using System;
using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region PersistenceWithAzure

        var persistence = endpointConfiguration.UsePersistence<AzureTablePersistence>();
        persistence.ConnectionString("DefaultEndpointsProtocol=https;AccountName=[ACCOUNT];AccountKey=[KEY];");

        #endregion

        #region RegisterLogicalBehavior

        endpointConfiguration.Pipeline.Register(new RegisterMyBehavior());

        #endregion
    }

    #region PersistenceWithAzureHost

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            var persistence = endpointConfiguration.UsePersistence<AzureTablePersistence>();
            persistence.ConnectionString("DefaultEndpointsProtocol=https;AccountName=[ACCOUNT];AccountKey=[KEY];");
        }
    }

    #endregion

    void CustomizingAzurePersistenceAllConnections(EndpointConfiguration endpointConfiguration)
    {
        #region AzurePersistenceAllConnectionsCustomization

        var persistence = endpointConfiguration.UsePersistence<AzureTablePersistence>();
        persistence.ConnectionString("connectionString");

        #endregion
    }

    void CustomizingAzurePersistenceSubscriptions(EndpointConfiguration endpointConfiguration)
    {
        #region AzurePersistenceSubscriptionsCustomization

        var persistence = endpointConfiguration.UsePersistence<AzureTablePersistence, StorageType.Subscriptions>();
        persistence.ConnectionString("connectionString");
        persistence.TableName("tableName");

        // Added in Version 1.3
        persistence.CacheFor(TimeSpan.FromMinutes(1));

        #endregion
    }

    void CustomizingAzurePersistenceSagas(EndpointConfiguration endpointConfiguration)
    {
        #region AzurePersistenceSagasCustomization

        var persistence = endpointConfiguration.UsePersistence<AzureTablePersistence, StorageType.Sagas>();
        persistence.ConnectionString("connectionString");

        endpointConfiguration.EnableInstallers();

        #endregion
    }

    void CustomizingDefaultTableName(EndpointConfiguration endpointConfiguration)
    {
        #region SetDefaultTable

        var persistence = endpointConfiguration.UsePersistence<AzureTablePersistence>();
        persistence.ConnectionString("connectionString");
        persistence.DefaultTable("TableName");

        endpointConfiguration.EnableInstallers();

        #endregion
    }

    void EnableInstallersConfiguration(EndpointConfiguration endpointConfiguration)
    {
        #region EnableInstallersConfiguration

        var persistence = endpointConfiguration.UsePersistence<AzureTablePersistence>();
        persistence.ConnectionString("connectionString");
        persistence.DefaultTable("TableName");

        endpointConfiguration.EnableInstallers();

        #endregion
    }

    void EnableInstallersConfigurationOptingOutFromTableCreation(EndpointConfiguration endpointConfiguration)
    {
        #region EnableInstallersConfigurationOptingOutFromTableCreation

        var persistence = endpointConfiguration.UsePersistence<AzureTablePersistence>();
        persistence.ConnectionString("connectionString");
        persistence.DefaultTable("TableName");

        // make sure the table name specified in the DefaultTable exists when calling DisableTableCreation
        endpointConfiguration.EnableInstallers();
        persistence.DisableTableCreation();

        #endregion
    }
}

// to avoid host reference
interface IConfigureThisEndpoint
{
}