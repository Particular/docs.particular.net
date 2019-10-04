---
title: Azure Storage Persistence
summary: Using Azure Storage as persistence
reviewed: 2019-10-01
component: ASP
tags:
 - Azure
 - Persistence
 - Performance
 - Hosting
related:
 - persistence/upgrades/asp-saga-deduplication
 - persistence/upgrades/asp-saga-pruning
 - samples/azure/storage-persistence
redirects:
 - nservicebus/using-azure-storage-persistence-in-nservicebus
 - nservicebus/azure/azure-storage-persistence
 - nservicebus/azure-storage-persistence
---

Certain features of NServiceBus require persistence to permanently store data. Among them are subscription storage, sagas, and timeouts. Various storage options are available including Azure Storage Services.


### Enable Azure Storage Persistence

partial: config


### Saga correlation

partial: correlation

To ensure that only one saga can be created for a given correlation property value, secondary indexes are used. Entities for the secondary index are stored in the same table as a saga. When a saga is completed the secondary index entity is removed as well. It's possible, but highly unlikely, that the saga's completion can leave an orphaned secondary index record. This does not impact the behavior of the persistence as it can detect orphaned records, but may leave a dangling entity in a table with a following `WARN` entry in logs: `Removal of the secondary index entry for the following saga failed: {sagaId}`.

If migrating from Version 6.2.3 or below without applying [saga deduplication](/persistence/upgrades/asp-saga-deduplication.md) a `DuplicatedSagaFoundException` can be thrown when when creating secondary index entities. The exception message will include all the information to track down the error for example: 

```
Sagas of type MySaga with the following identifiers 'GUID1', 'GUID2' are considered duplicates because of the violation of the Unique property CorrelationId.
```

The upgrade guide linked above contains instructions.


### Supported saga properties' types

partial: saga-property-types


### Saga Correlation property restrictions

Saga correlation property values are subject to the underlying Azure Storage table `PartitionKey` and `RowKey` restrictions:

* Up to 1KB in size
* Cannot contain [invalid characters](https://docs.microsoft.com/en-us/rest/api/storageservices/Understanding-the-Table-Service-Data-Model#tables-entities-and-properties)
