---
title: Migration from Azure Table Persistence
component: CosmosDB
related:
- persistence/azure-table
reviewed: 2020-11-17
---

For existing system running in Azure and using [Azure Table Persistence](/persistence/azure-table) where a migration to Azure Cosmos DB SQL API is desired, a multi-step migration process is recommended, using Particular and Azure Cosmos DB tools.

WARN: The endpoint being migrated must be offline while migrating saga data. The saga data must be using secondary indexes (introduced in Azure Table Persistence 2.x) or be stored with Azure Table Persistence Version 3 or higher for this upgrade guide to succeed. The migration scenario described assumes only saga data of a one saga is stored per table.

For each Azure Table Persistence table containing saga data, the following four major steps must be performed:

1. [Export saga data from Table Storage](#export-data)
1. [Inspect exported saga data](#data-inspection) for quality
1. [Import data](#import-data) into Cosmos DB
1. [Inspect imported saga data](#data-inspection) for quality

Prior to starting the endpoint, configure the endpoint to [use migration mode](#using-migration-mode).

## Export data

To export data from Table Storage, a .NET tool provided by Particular is required. Install the tool from MyGet using the following command:

```
dotnet tool install Particular.Asp.Export --tool-path <installation-path> --version 0.*
```

Once installed, the `export-aspsagas` command line tool will be available for use at the installation path used earlier. For example:

```
export-aspsagas -c "UseDevelopmentStorage=true" -s OrderSagaData
```

`<exported-path>` is the destination path where the `export-aspsagas.exe` will be found.

Once the tool is executed, saga data for the selected saga data table will be stored in the current working directory as a sub-folder named after the saga data class with each saga data record as individual JSON files. These files can be inspected and imported into Cosmos DB using the [instructions below](#import-data).

### Export tool options

`-c` | `--connectionstring`: Set the connection string to the Table Storage<br>
`-s` | `--sagadataname`: The saga data class name without the namespace (e.g. `OrderSagaData`) of the saga data to export. This will be used to derive the table storage name.<br>
`-i | --ignore-updates`: Allow use of the tool even if a newer version is available.<br>
`-v | --verbose`: Enable verbose output.<br>
`--version`: Show the current version of the tool.

#### Updating the tool

The tool can be updated with the following command:

```
dotnet tool update --tool-path <installation-path> Particular.Asp.Export --version 0.*
```

### Exported saga ID

As part of the export process a new saga ID is generated for each saga that is compliant with the `NServiceBus.Persistence.CosmosDB` package. The original saga ID is stored in the `_NServiceBus-Persistence-Metadata.SagaDataContainer-MigratedSagaId` metadata property embedded in the exported saga data.

### Export tool customizations

For customers that require a certain degree of customization, the .NET tool code is publicly available at https://github.com/Particular/NServiceBus.Persistence.CosmosDB and can be forked.

## Import data

The exported saga data JSON files can be imported into Cosmos DB using the [Data migration tool](https://docs.microsoft.com/en-us/azure/cosmos-db/import-data#Install) provided by Microsoft. The import tool provides both [a UI and a command line](https://docs.microsoft.com/en-us/azure/cosmos-db/import-data#AzureTableSource) option.

For example, to import a single saga data table called `OrderSagaData` originally exported to the location `C:\path\to\OrderSagaData`, the following command is required:

```
dt.exe /s:JsonFile /s.Files:C:\\path\\to\\OrderSagaData\\*.* /t:DocumentDB /t.IdField:id /t.DisableIdGeneration /t.Collection:OrderSagaData /t.PartitionKey:/id /t.ConnectionString:AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==;database=CosmosDBPersistence
```
where the following parameters must be adjusted:

`s.Files`: file path to the folder containing a specific saga data JSON files exported from Table Storage.<br/>
`t.Collection`: Cosmos DB collection to be used for the imported data.<br/>
`t.ConnectionString`: Cosmos DB connection string including Cosmos SB database name (e.g`;database=MyCosmosDb`).<br/>

## Data inspection

Due to the [limited types](https://docs.microsoft.com/en-us/rest/api/storageservices/understanding-the-table-service-data-model#property-types) supported by Azure Storage Tables, some types are stored in the table by the [Azure Storage persister](/persistence/azure-table) as serialized JSON strings. The export tool makes a _best effort_ to re-serialize these values for import into Cosmos DB. As a result, the data can and should be inspected for quality both before and after the import. The migrated endpoint and all saga types migrated should be thoroughly tested before moving into production to ensure the migration is correct.

WARN: Dates stored using `DateTimeOffset` data type are susceptible to incorrect translation. Saga data storing properties using `DateTimeOffset` should be verified after saga data import is completed to ensure accurate conversion.

## Using migration mode

[Auto-correlated messages](/nservicebus/sagas/message-correlation.md#auto-correlation) include the saga ID in the [message headers](/nservicebus/messaging/headers.md#saga-related-headers-replying-to-a-saga). For unprocessed auto-correlated messages sent prior to migration, this may result in a saga not found error, since the saga ID contained in the message headers is not the [new saga ID](#export-data-exported-saga-id) expected by the Cosmos DB persister.

By enabling migration mode only for auto correlated messages, the saga persister will attempt to query the collection using the [original saga ID in the saga metadata](#export-data-exported-saga-id) when the saga is not found. Messages [explicitly mapped using the `ConfigureHowToFindSaga` method](/nservicebus/sagas/message-correlation.md) do not require the additional query.

NOTE: Querying by the original saga ID with migration mode will incur additional RU usage on the collection.

NOTE: [Saga timeouts](/nservicebus/sagas/timeouts.md) always use auto-correlation.

To enable migration mode use the configuration option:

snippet: CosmosDBMigrationMode

Migration mode can be disabled when all auto-correlated pre-migration messages have been processed, or when all migrated sagas have [ended](/nservicebus/sagas/#ending-a-saga).

WARN: Inspecting the endpoint's queue may not be enough to determine if all pre-migration auto-correlated messages have been processed. Messages that are sent via [delayed delivery](/nservicebus/messaging/delayed-delivery.md), including all saga timeouts, may not be visible in the endpoint's queue.

### Counting migrated sagas

Perform the following query on each collection containing migrated saga data:

```SQL
SELECT COUNT(c["_NServiceBus-Persistence-Metadata"]["SagaDataContainer-MigratedSagaId"]) AS SagasInMigration FROM c
```

This will produce a result that returns the number of migrated sagas still open in the collection:

```JSON
[
    {
        "SagasInMigration": 2
    }
]
```

Migration mode can be disabled when the query result returns 0 for all collections that contained migrated saga data.
