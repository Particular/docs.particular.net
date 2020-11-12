---
title: Azure Storage Persistence
summary: Using Azure Storage as persistence
reviewed: 2019-10-01
component: ASP
related:
 - persistence/upgrades/asp-saga-deduplication
 - persistence/upgrades/asp-saga-pruning
 - samples/azure/storage-persistence
redirects:
 - nservicebus/using-azure-storage-persistence-in-nservicebus
 - nservicebus/azure/azure-storage-persistence
 - nservicebus/azure-storage-persistence
 - persistence/azure-storage
---

Certain features of NServiceBus require persistence to permanently store data. Among them are subscription storage, sagas, and outbox. Various storage options are available including Azure Table and Azure Cosmos DB Table API.

Azure Storage Persistence stores NServiceBus data in [Azure Table storage](https://azure.microsoft.com/en-us/services/storage/tables/) or [Azure Cosmos DB using the Table API](https://docs.microsoft.com/en-us/azure/cosmos-db/table-support/).

## Persistence at a glance

For a description of each feature, see the [persistence at a glance legend](/persistence/#persistence-at-a-glance).

|Feature                    |   |
|:---                       |---
|Supported storage types    |Sagas, Outbox, Subscriptions
|Transactions               |Using TransactionalBatch, [with caveats](transactions.md)
|Concurrency control        |Optimistic concurrency
|Scripted deployment        |Not supported
|Installers                 |Supported. Subscription, the default table or saga tables derived by convention when no default table is set are created at runtime, when enabled.


### Enable Azure Table Persistence

partial: config


### Saga correlation

partial: correlation

To ensure that only one saga can be created for a given correlation property value, secondary indexes are used. Entities for the secondary index are stored in the same table as a saga. When a saga is completed the secondary index entity is removed as well. It's possible, but highly unlikely, that the saga's completion can leave an orphaned secondary index record. This does not impact the behavior of the persistence as it can detect orphaned records, but may leave a dangling entity in a table with a following `WARN` entry in logs: `Removal of the secondary index entry for the following saga failed: {sagaId}`.

If migrating from Version 6.2.3 or below without applying [saga deduplication](/persistence/upgrades/asp-saga-deduplication.md) a `DuplicatedSagaFoundException` can be thrown when when creating secondary index entities. The exception message will include all the information to track down the error for example: 

```
Sagas of type MySaga with the following identifiers 'GUID1', 'GUID2' are considered duplicates because of the violation of the Unique property CorrelationId.
```

The upgrade guide linked above contains instructions.


### Saga concurrency

When simultaneously handling messages, conflicts may occur. See below for examples of the exceptions which are thrown. _[Saga concurrency](/nservicebus/sagas/concurrency.md)_ explains how these conflicts are handled, and contains guidance for high-load scenarios.

#### Starting a saga

Example exception:

```
NServiceBus.Persistence.AzureStorage.RetryNeededException: This operation requires a retry as it wasn't possible to successfully process it now.
```

#### Updating or deleting saga data

Azure Storage Persistence uses [optimistic concurrency control](https://en.wikipedia.org/wiki/Optimistic_concurrency_control) when updating or deleting saga data.

Example exception:

```
Microsoft.WindowsAzure.Storage.StorageException: Element 0 in the batch returned an unexpected response code.

Request Information
RequestID:010c234e-3002-0145-06eb-72b85a000000
RequestDate:Tue, 24 Sep 2019 15:16:45 GMT
StatusMessage:The update condition specified in the request was not satisfied.
ErrorCode:
ErrorMessage:The update condition specified in the request was not satisfied.
RequestId:010c234e-3002-0145-06eb-72b85a000000
Time:2019-09-24T15:16:46.0746310Z
```

### Supported saga properties' types

partial: saga-property-types


### Saga Correlation property restrictions

Saga correlation property values are subject to the underlying Azure Storage table `PartitionKey` and `RowKey` restrictions:

* Up to 1KB in size
* Cannot contain [invalid characters](https://docs.microsoft.com/en-us/rest/api/storageservices/Understanding-the-Table-Service-Data-Model#tables-entities-and-properties)