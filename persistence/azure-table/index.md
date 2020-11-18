---
title: Azure Table Persistence
summary: Using Azure Tables as persistence
reviewed: 2019-10-01
component: ASP
related:
 - persistence/upgrades/asp-saga-deduplication
 - persistence/upgrades/asp-saga-pruning
 - samples/azure/azure-table
redirects:
 - nservicebus/using-azure-storage-persistence-in-nservicebus
 - nservicebus/azure/azure-storage-persistence
 - nservicebus/azure-storage-persistence
 - persistence/azure-table
---

Certain features of NServiceBus require persistence to permanently store data. Among them are subscription storage, sagas, and outbox. Various storage options are available including Azure Table and Azure Cosmos DB Table API.

Azure Table Persistence stores NServiceBus data in [Azure Table storage](https://azure.microsoft.com/en-us/services/storage/tables/) or [Azure Cosmos DB using the Table API](https://docs.microsoft.com/en-us/azure/cosmos-db/table-support/).

## Persistence at a glance

For a description of each feature, see the [persistence at a glance legend](/persistence/#persistence-at-a-glance).

|Feature                    |   |
|:---                       |---
|Supported storage types    |Sagas, Outbox, Subscriptions
|Transactions               |Using TransactionalBatch, [with caveats](transactions.md)
|Concurrency control        |Optimistic concurrency
|Scripted deployment        |Not supported
|Installers                 |Supported. Subscription, the default table or saga tables derived by convention when no default table is set are created at runtime, when enabled.


## Enable Azure Table Persistence

partial: config


### Transactions
TODO

The Cosmos DB persister supports using the Cosmos DB transactional batch API. However, Cosmos DB only allows operations to be batched if all operations are performed within the same logical partition key. This is due to the distributed nature of the Cosmos DB service, which does not support distributed transactions.

The transactions documentation provides additional details on how to configure NServiceBus to resolve the incoming message to a specific partition key to take advantage of this Cosmos DB feature.

partial: correlation

### Saga concurrency

partial: saga-concurrency

### Supported saga properties' types

partial: saga-property-types


### Saga Correlation property restrictions

Saga correlation property values are subject to the underlying Azure Storage table `PartitionKey` and `RowKey` restrictions:

* Up to 1KB in size
* Cannot contain [invalid characters](https://docs.microsoft.com/en-us/rest/api/storageservices/Understanding-the-Table-Service-Data-Model#tables-entities-and-properties)