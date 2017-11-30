---
title: Azure Storage Persistence
summary: Using Azure Storage as persistence
reviewed: 2016-05-12
component: ASP
tags:
 - Azure
 - Persistence
 - Performance
 - Hosting
related:
 - persistence/upgrades/asp-saga-deduplication
 - persistence/upgrades/asp-saga-pruning
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

To ensure that only one saga can be created for one correlation property value, secondary indexes have been introduced. Their entities are stored in the same table as a saga. When a saga is completed, a secondary index entity is removed as well. It's possible, but highly unlikely, that the saga's completion can leave an orphaned secondary index record. This does not impact the behavior of the persistence as it can detect orphaned records, but may leave a dangling entity in a table with a following `WARN` entry in logs: `Removal of the secondary index entry for the following saga failed: {sagaId}`.

If a migration from 6.2.3 or earlier to a newer version was performed without applying [saga deduplication](/persistence/upgrades/asp-saga-deduplication.md), `DuplicatedSagaFoundException` can be observed because of duplicates violating a unique property of a saga. The exception message will include all the information to track down the error for example: 

```
Sagas of type MySaga with the following identifiers 'GUID1', 'GUID2' are considered duplicates because of the violation of the Unique property CorrelationId.
```

The way to address them is go through the upgrade guide linked above.


### Supported saga properties' types

partial: saga-property-types


### Saga Correlation property restrictions

Saga correlation property values are subject to the underlying Azure Storage table `PartitionKey` and `RowKey` restrictions:
* Up to 1KB in size
* Cannot contain [invalid characters](https://docs.microsoft.com/en-us/rest/api/storageservices/Understanding-the-Table-Service-Data-Model#tables-entities-and-properties)
