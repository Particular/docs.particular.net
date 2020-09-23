---
title: Migration from Azure Storage Persistence
component: CosmosDB
related:
- persistence/azure-storage
reviewed: 2020-09-20
---

For existing system running in Azure and using [Azure Storage Persistence](/persistence/azure-storage), a three-step migration process is recommended, using Particular and Azure Cosmos DB tools.

The migration would include three major steps required for **each** Azure Storage Persistence table containing saga data:

1. Export saga data from Table Storage
1. Import data into
1. Inspect migrated data for quality

## Export data

To export data from Table Storage, a .NET tool provided by Particular is required. The tool can be obtained from MyGet and installed using the following command:

```
dotnet tool install -g Particular.AzureStoragePersistenceSagaExporter --add-source=https://www.myget.org/F/particular/api/v3/index.json
```

Once installed, the `export-aspsagas` command line tool will be available for use.

`export-aspsagas [options]`

Once the tool is executed, saga data for the selected saga data table will be stored as a folder named after the saga data class with all the records as JSON files. These files can be inspected and imported into Cosmos DB using the instructions below.

### Export tool options
 
`-c` | `--connectionstring` : Set the connection string to the table storage

`-s` | `--sagadataname`: The saga data class name without the namespace (e.g. `OrderSagaData`) of the saga data to export. This will be used to derive the table storage name from.

### Export tool customizations

For customers that require a certain degree of customization, the .NET tool code is publicly available at https://github.com/Particular/NServiceBus.Persistence.CosmosDB and can be forked.

## Import data

The exported saga data and stored as JSON files can be imported into Cosmos DB using the [Data migration tool](https://docs.microsoft.com/en-us/azure/cosmos-db/import-data#Install) provided by Microsoft. The import tool provides a [UI and a command line](https://docs.microsoft.com/en-us/azure/cosmos-db/import-data#AzureTableSource) options.

For example, to import a single saga data table called `OrderSagaData` originally exported to the location `C:\path\to\OrderSagaData`, the following command is required:

```
dt.exe /s:JsonFile /s.Files:C:\\path\\to\\OrderSagaData\\*.* /t:DocumentDB /t.IdField:id /t.DisableIdGeneration /t.Collection:OrderSagaData /t.PartitionKey:/id /t.ConnectionString:AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==;database=CosmosDBPersistence
```
where the following parameters need to be adjusted:

`s.Files`: file path to the folder containing a specific saga data JSON files exported from Table Storage.<br/>
`t.Collection`: Cosmos DB collection to be used for the imported data.<br/>
`t.ConnectionString`: Cosmos DB connection string including Cosmos SB database name (e.g`;database=MyCosmosDb`).<br/>

## Data inspection

The data can and should be inspected for quality either before or after the import.

WARN: Dates stored using `DateTimeOffset` data type are susceptible to incorrect translation. Saga data storing properties using `DateTimeOffset` should be verified after saga data import is completed to ensure accurate conversion.

## Completing the migration

Perform the following query on each collection containing migrated saga data:

```SQL
SELECT COUNT(c["_NServiceBus-Persistence-Metadata"]["SagaDataContainer-MigratedSagaId"]) AS SagasInMigration FROM c
```

This will produce a result that returns the number of migrated sagas still open:

```JSON
[
    {
        "SagasInMigration": 2
    }
]
```

Migration mode can be disabled when the query result returns 0 for all collections containing migrated saga data.
