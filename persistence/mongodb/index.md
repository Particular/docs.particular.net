---
title: MongoDB Persistence
component: mongodb
versions: '[2,)'
related:
- samples/mongodb
redirects:
- persistence/mongodb-tekmaven
- nservicebus/messaging/databus/mongodb-tekmaven
reviewed: 2019-07-20
---

Uses the [MongoDB document database](https://www.mongodb.com/) for storage.

NOTE: NServiceBus.Storage.MongoDB supports MongoDB server versions 3.6 and higher

## Persistence at a glance

For a description of each feature, see the [persistence at a glance legend](/persistence/#persistence-at-a-glance).

|Feature                    |   |
|:---                       |---
|Supported storage types    |Sagas, Outbox, Subscriptions
|Transactions               |Enabled and required by default
|Concurrency control        |Pessimistic concurrency only
|Scripted deployment        |Not supported
|Installers                 |None. Documents are created in database at runtime as needed.


## Usage

Add a NuGet package reference to `NServiceBus.Storage.MongoDB`. Configure the endpoint to use the persistence through the following configuration API:

snippet: MongoDBUsage

### Customizing the connection

By default, a `MongoClient` is created that connects to `mongodb://localhost:27017` and uses the endpoint name as its database name.

Customize the server, port, and authentication database using the following configuration API:

snippet: MongoDBClient

Specify the database to use for NServiceBus documents using the following configuration API:

snippet: MongoDBDatabaseName

## Transactions

MongoDB [transactions](https://docs.mongodb.com/manual/core/transactions/) are enabled and required by default. This allows the persister to use pessimistic locking and to update multiple saga instances and commit them atomically during message processing.

WARN: MongoDB transactions require a replica set or sharded cluster. Refer to the [MongoDB transaction documentation](https://docs.mongodb.com/manual/core/transactions/#transactions-and-atomicity) for more information about supported configurations and required MongoDB server versions.

NOTE: The MongoDB persister supports transactions on shared clusters starting from version 2.1.

### Disabling transactions

The following configuration API is available for compatibility with MongoDB server configurations which don't support transactions:

snippet: MongoDBDisableTransactions

Note that this disables the ability to use pessimistic locking for sagas which might result in higher contention in the database.

### Shared transactions

NServiceBus supports sharing MongoDB transactions between Saga persistence and business data. The shared transaction can be used to persist document updates for both concerns atomically.

To use the shared transaction in a message handler:

snippet: MongoDBHandlerSharedTransaction

WARN: In order to participate in the shared transaction the MongoDB session must be passed into collection API calls as demonstrated above.

#### Testing

The `TestableMongoSynchronizedStorageSession` class in the `NServiceBus.Testing` namespace has been provided to facilitate [testing a handler](/nservicebus/testing/) that utilizes the shared transaction feature.

## Outbox cleanup 

When the outbox is enabled, the deduplication data is kept for seven days by default. To customize this time frame, use the following API:

snippet: MongoDBOutboxCleanup


## Saga concurrency

When simultaneously handling messages, conflicts may occur. See below for examples of the exceptions which are thrown. _[Saga concurrency](/nservicebus/sagas/concurrency.md)_ explains how these conflicts are handled, and contains guidance for high-load scenarios.

### Starting a saga

Example exception:

```
MongoDB.Driver.MongoCommandException: Command insert failed: WriteConflict.
```

### Updating or deleting saga data

Starting from version 2.2, MongoDB persistence uses [exclusive locks](https://docs.mongodb.com/manual/faq/concurrency/) when updating or deleting saga data. The saga persister tries to acquire an exclusive lock on the saga data for up to 60 seconds. If, within this time period, an exclusive lock cannot be acquired, a `TimeoutException` is thrown and regular message retry policies are applied.

Example exception:

```
System.TimeoutException: Unable to acquire exclusive write lock for saga on collection 'collectionName'
```

In versions prior to version 2.2, MongoDB persistence uses [optimistic concurrency control](https://en.wikipedia.org/wiki/Optimistic_concurrency_control) when updating or deleting saga data.

Example exception:

```
MongoDB.Driver.MongoCommandException: Command update failed: WriteConflict.
```

include: saga-concurrency