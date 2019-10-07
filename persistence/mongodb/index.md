---
title: MongoDB Persistence
component: mongodb
versions: '[1,)'
tags:
- Persistence
related:
- samples/mongodb
redirects:
- persistence/mongodb-tekmaven
- nservicebus/messaging/databus/mongodb-tekmaven
reviewed: 2019-07-20
---

Uses the [MongoDB document database](https://www.mongodb.com/) for storage.

NOTE: NServiceBus.Storage.MongoDB supports MongoDB server versions 3.6 and higher

## Supported persistence types

* [Subscriptions](/nservicebus/messaging/publish-subscribe/)
* [Sagas](/nservicebus/sagas/)
* [Outbox](/nservicebus/outbox/)


## Usage

Add a NuGet package reference to `NServiceBus.Storage.MongoDB`. Configure the endpoint to use the persistence through the following configuration API:

snippet: MongoDBUsage

### Customizing the connection

By default, a `MongoClient` is created that connects to `mongodb://localhost:27017` and uses the endpoint name as its database name. This default connection is used for all the persisters.

Customize the server, port, and authentication database using the following configuration API:

snippet: MongoDBClient

Specify the database to use for NServiceBus documents using the following configuration API:

snippet: MongoDBDatabaseName

## Multi-document transactions

Multi-document [transactions](https://docs.mongodb.com/manual/core/transactions/) are enabled and required by default. This allows the package to support updating multiple saga instances and commit them all atomically during message processing.

WARN: MongoDB transactions are only supported on MongoDB server replica sets.

### Disabling transactions

The following configuration API is available for compatibility with MongoDB server versions less than 4 or for use with sharded clusters:

snippet: MongoDBDisableTransactions

### Shared transactions

NServiceBus supports sharing the same MongoDB session transaction between Saga persistence and business data. A single session transaction can be used to persist document updates for both concerns atomically.

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

MongoDB persistence uses [optimistic concurrency control](https://en.wikipedia.org/wiki/Optimistic_concurrency_control) when updating or deleting saga data.

Example exception:

```
MongoDB.Driver.MongoCommandException: Command update failed: WriteConflict.
```
