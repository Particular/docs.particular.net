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
 - nservicebus/upgrades/asp-saga-deduplication
 - nservicebus/upgrades/asp-saga-pruning
redirects:
 - nservicebus/using-azure-storage-persistence-in-nservicebus
 - nservicebus/azure/azure-storage-persistence
---

Certain features of NServiceBus require persistence to permanently store data. Among them are subscription storage, sagas, and timeouts. Various storage options are available including Azure Storage Services.


### How to enable persistence with Azure Storage Services

First add a reference to the assembly that contains the Azure storage persisters. When working with NServiceBus Version 5 or lower this is done by adding a NuGet package reference to `NServiceBus.Azure`. For NServiceBus Version 6 and above the NuGet package reference is `NServiceBus.Persistence.AzureStorage`.

If self hosting, configure the persistence technology using the configuration API.

snippet: PersistanceWithAzure


### Saga correlation

NOTE: In Versions 6 and above of NServiceBus, all correlated properties are [unique by default](/nservicebus/upgrades/5to6/handlers-and-sagas.md#saga-api-changes-unique-attribute-no-longer-needed) so there is no longer a configuration setting.

One of the limitations of the Azure Storage Persistence is support for only one `[Unique]` property (a saga property which value is guaranteed to be unique across all sagas of this type).

To ensure that only one saga can be created for one correlation property value, secondary indexes have been introduced. Their entities are stored in the same table as a saga. When a saga is completed, a secondary index entity is removed as well. It's possible, but highly unlikely, that the saga's completion can leave an orphaned secondary index record. This does not impact the behavior of the persistence as it can detect orphaned records, but may leave a dangling entity in a table with a following `WARN` entry in logs: `Removal of the secondary index entry for the following saga failed: {sagaId}`.

If a migration from 6.2.3 or earlier to a newer version was performed without applying [saga deduplication](/nservicebus/upgrades/asp-saga-deduplication.md), `DuplicatedSagaFoundException` can be observed because of duplicates violating a unique property of a saga. The exception message will include all the information to track down the error for example: 

```no-highlight
Sagas of type MySaaga with the following identifiers 'GUID1', 'GUID2' are considered duplicates because of the violation of the Unique property CorrelationId.`
```

The way to address them is go through the upgrade guide linked above.


### Supported saga properties' types

Azure Storage Persistence supports exactly the same set of types as [Azure Table Storage](https://docs.microsoft.com/en-us/rest/api/storageservices/fileservices/Understanding-the-Table-Service-Data-Model). When a saga containing a property of an unsupported type is persisted, an exception containing a following information is thrown: `The property type 'the_property_name' is not supported in windows azure table storage`. If an object of a non-supported type is required to be stored, then it's a user responsibility to serialize/deserialize the value.