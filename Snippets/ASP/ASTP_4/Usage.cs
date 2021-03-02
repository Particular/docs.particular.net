using System;
using System.Globalization;
using System.IO;
using Microsoft.Azure.Cosmos.Table;
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
        CloudTableClient cloudTableClient = null;

        #region AzurePersistenceSagasCustomization

        var persistence = endpointConfiguration.UsePersistence<AzureTablePersistence, StorageType.Sagas>();
        persistence.ConnectionString("connectionString");
        // or
        persistence.UseCloudTableClient(cloudTableClient);

        #endregion
    }

    void CustomizingAzurePersistenceSagasCompatibility(EndpointConfiguration endpointConfiguration)
    {
        #region AzurePersistenceSagasCompatibility

        var persistence = endpointConfiguration.UsePersistence<AzureTablePersistence, StorageType.Sagas>();

        var compatibility = persistence.Compatibility();
        // e.g. Disable secondary index
        compatibility.DisableSecondaryKeyLookupForSagasCorrelatedByProperties();

        #endregion
    }

    public void ConfigureCustomJsonSerializerSettings(EndpointConfiguration endpointConfiguration)
    {
        #region AzurePersistenceSagasJsonSerializerSettings
        var persistence = endpointConfiguration.UsePersistence<AzureTablePersistence, StorageType.Sagas>();

        persistence.JsonSettings(new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
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