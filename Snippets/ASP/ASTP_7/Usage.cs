using Azure.Data.Tables;
using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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

        // or

        TableServiceClient tableServiceClient = new TableServiceClient("connectionString");
        persistence.UseTableServiceClient(tableServiceClient);

        #endregion
    }

    public void ConfigureCustomJsonSerializerSettings(EndpointConfiguration endpointConfiguration)
    {
        #region AzurePersistenceSagasJsonSerializerSettings
        var persistence = endpointConfiguration.UsePersistence<AzureTablePersistence, StorageType.Sagas>();

        persistence.JsonSettings(new JsonSerializerSettings
        {
            Converters =
                {
                    new IsoDateTimeConverter
                    {
                        DateTimeStyles = DateTimeStyles.RoundtripKind
                    }
                }
        });
        #endregion
    }

    public void ConfigureCustomJsonReader(EndpointConfiguration endpointConfiguration)
    {
        #region AzurePersistenceSagasReaderCreator
        var persistence = endpointConfiguration.UsePersistence<AzureTablePersistence, StorageType.Sagas>();

        persistence.ReaderCreator(
            readerCreator: textReader =>
            {
                return new JsonTextReader(textReader);
            });
        #endregion
    }

    public void ConfigureCustomJsonWriter(EndpointConfiguration endpointConfiguration)
    {
        #region AzurePersistenceSagasWriterCreator

        var persistence = endpointConfiguration.UsePersistence<AzureTablePersistence, StorageType.Sagas>();

        persistence.WriterCreator(
            writerCreator: writer =>
            {
                return new JsonTextWriter(writer)
                {
                    Formatting = Formatting.None
                };
            });

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