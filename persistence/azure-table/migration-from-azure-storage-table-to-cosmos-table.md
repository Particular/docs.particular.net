---
title: Migration from Azure Storage Table to Azure Cosmos DB Table API
component: ASP
related:
- persistence/azure-table
reviewed: 2020-11-17
---

WARN: The endpoint being migrated must be offline while migrating saga data. The saga data must be using secondary indexes (introduced in Azure Table Persistence 2.x) or be stored with Azure Table Persistence Version 3 or higher for this upgrade guide to succeed. The migration scenario described assumes only saga data of a one saga is stored per table.

## Import data

NOTE: At the time of writing this guidance the Data migration tool did incorrectly project columns and thus would crash with `NullReferenceException`. The [Pullrequest](https://github.com/Azure/azure-documentdb-datamigrationtool/pull/126) has been merged but it is not confirmed yet when the tool will be released. If required build the latest master branch of the tool.

The saga data can be imported into Cosmos DB Table API using the [Data migration tool](https://docs.microsoft.com/en-us/azure/cosmos-db/import-data#Install) provided by Microsoft. The import tool provides both [a UI and a command line](https://docs.microsoft.com/en-us/azure/cosmos-db/import-data#AzureTableSource) option. The general command looks like the following

```
dt.exe /s:AzureTable /s.ConnectionString:"<AzureTableStorageConnectionString>" /s.Table:<SagaTableName> /s.InternalFields:All /s.Projection:"<SagaProperties>;Originator;OriginalMessageId;NServiceBus_2ndIndexKey;SagaId" /t:TableAPIBulk /t.ConnectionString:"<AzureCosmosTableApiConnectionString>" /t.TableName:<SagaTableName> /ErrorLog:errors.csv /ErrorDetails:All /OverwriteErrorLog:true
```

### Parameters

`<AzureTableStorageConnectionString>`: The Azure Table Storage (source) connection string<br/>
`<AzureCosmosTableApiConnectionString>`: The Azure Cosmos DB Table API (destination) connection string.<br/>
`<SagaProperties>`: A semicolon seperated list of all saga properties that need to be projected (e.g `OrderId;OrderDescription;OrderState`). Make sure to leave `Originator;OriginalMessageId;NServiceBus_2ndIndexKey;SagaId` since those are standard columns that always need to be projected in case they are available.<br/>
`<SagaTableName>`: The name of the saga data table (e.g `OrderSagaData`).<br/>

### Example

For example, to import a single saga data table called `OrderSagaData` with the saga data type:

```
public class OrderSagaData : IContainSagaData
{
    public Guid Id { get; set; }
    public string Originator { get; set; }
    public string OriginalMessageId { get; set; }
    public Guid OrderId { get; set; }
    public string OrderDescription { get; set; }
    public OrderState OrderState { get; set; }
}
```

the following command can be used:

```
dt.exe /s:AzureTable /s.ConnectionString:"<_>AzureTableStorageConnectionString>" /s.Table:OrderSagaData /s.InternalFields:All /s.Projection:"OrderId;OrderDescription;OrderState;Originator;OriginalMessageId;NServiceBus_2ndIndexKey;SagaId" /t:TableAPIBulk /t.ConnectionString:"<AzureCosmosTableApiConnectionString>" /t.TableName:OrderSagaData /ErrorLog:errors.csv /ErrorDetails:All /OverwriteErrorLog:true
```

## Data inspection

Due to the [limited types](https://docs.microsoft.com/en-us/rest/api/storageservices/understanding-the-table-service-data-model#property-types) supported by Azure Storage Tables, some types are stored in the table by the [Azure Table persister](/persistence/azure-table) as serialized JSON strings. The data can and should be inspected for quality both before and after the import. The migrated endpoint and all saga types migrated should be thoroughly tested before moving into production to ensure the migration is correct.