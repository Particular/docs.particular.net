---
title: Migration from Azure Storage Table to Azure Cosmos DB Table API
component: ASP
related:
- persistence/azure-table
reviewed: 2025-11-24
---

> [!WARNING]
> The endpoint being migrated must be offline while migrating saga data. The migration scenario described assumes only saga data of one saga type is stored per table.

## Import data

The saga data can be imported into Cosmos DB Table API using the [Azure Cosmos DB Desktop Data Migration Tool](https://github.com/azurecosmosdb/data-migration-desktop-tool) provided by Microsoft. This tool is built on .NET 6, is cross-platform, and replaces the legacy `dt.exe` tool.

### Download the tool

Download the latest version of the Azure Cosmos DB Desktop Data Migration Tool from the [GitHub releases page](https://github.com/azurecosmosdb/data-migration-desktop-tool/releases).

### Configure the migration

Create a `migrationsettings.json` file in the tool's directory with the following structure:

```json
{
  "Source": "AzureTableAPI",
  "Sink": "AzureTableAPI",
  "SourceSettings": {
    "ConnectionString": "<AzureTableStorageConnectionString>",
    "Table": "<SagaTableName>"
  },
  "SinkSettings": {
    "ConnectionString": "<AzureCosmosTableApiConnectionString>",
    "Table": "<SagaTableName>"
  },
  "Operations": []
}
```

### Parameters

`<AzureTableStorageConnectionString>`: The Azure Table Storage (source) connection string. This can be found in the Azure Portal under your Storage Account's **Access keys** or **Connection string** section.<br/>
`<AzureCosmosTableApiConnectionString>`: The Azure Cosmos DB Table API (destination) connection string. This can be found in the Azure Portal under your Cosmos DB account's **Connection String** or **Keys** section. Ensure the connection string includes the `TableEndpoint` parameter pointing to your Cosmos DB Table API account.<br/>
`<SagaTableName>`: The name of the saga data table (e.g `OrderSagaData`).<br/>

> [!NOTE]
> The migration tool automatically migrates all columns from the source table, including all saga properties and the standard NServiceBus columns (`Originator`, `OriginalMessageId`, etc.). No explicit column projection is required.

### Example

For example, to import a single saga data table called `OrderSagaData` with the saga data type:

```
public class OrderSagaData : ContainSagaData
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public string OrderDescription { get; set; }
    public OrderState OrderState { get; set; }
}
```

the following `migrationsettings.json` can be used:

```json
{
  "Source": "AzureTableAPI",
  "Sink": "AzureTableAPI",
  "SourceSettings": {
    "ConnectionString": "DefaultEndpointsProtocol=https;AccountName=MyStorageAccount;AccountKey=<key>;EndpointSuffix=core.windows.net",
    "Table": "OrderSagaData"
  },
  "SinkSettings": {
    "ConnectionString": "DefaultEndpointsProtocol=https;AccountName=MyCosmosAccount;AccountKey=<key>;TableEndpoint=https://MyCosmosAccount.table.cosmos.azure.com:443/",
    "Table": "OrderSagaData"
  },
  "Operations": []
}
```

### Run the migration

Execute the migration tool from the command line:

```bash
dmt.exe
```

The tool will automatically read the `migrationsettings.json` file from the current directory and begin the migration process. The tool will migrate all data from the source table to the destination table, preserving all columns and their values.

## Data inspection

Due to the [limited types](https://docs.microsoft.com/en-us/rest/api/storageservices/understanding-the-table-service-data-model#property-types) supported by Azure Storage Tables, some types are stored in the table by the [Azure Table persister](/persistence/azure-table) as serialized JSON strings. The data can and should be inspected for quality both before and after the import. The migrated endpoint and all saga types migrated should be thoroughly tested before moving into production to ensure the migration is correct.