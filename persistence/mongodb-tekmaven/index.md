---
title: MongoDB Persistence (Ryan Hoffman)
summary: A community persistence that uses MongoDB as the data store
component: MongoPersistenceTekmaven
reviewed: 2018-09-14
versions: '[2,)'
tags:
 - Persistence
related:
 - samples/mongodb
 - nservicebus/messaging/databus/mongodb-tekmaven
redirects:
 - nservicebus/mongodb-persistence-tekmaven
---

Uses [MongoDB](https://www.mongodb.com/) for storage.

Includes MongoDB persistence implementations for

 * [Timeouts](/nservicebus/sagas/timeouts.md)
 * Subscriptions
 * [Sagas](/nservicebus/sagas/)


## Usage

To use MongoDB for persistence

snippet: MongoUsage


## Connection settings

There are several ways to set the MongoDB Connection


### Via code

This allows resolution of configuration setting at run-time.

snippet: MongoConnectionString


### Via an app.config connection string

snippet: MongoConnectionStringAppConfig

Override the default connection string name as follows:

snippet: MongoConnectionStringName


## Saga definition guideline

For sagas to work correctly, the following must be enforced:

 * Saga data should implement `IContainSagaData`.
 * Requires a property `Version` decorated with attribute `[DocumentVersion]`.

For example:

snippet: MongoSampleSaga


## Dealing with concurrency

The key concurrency safeguards that sagas guarantee depend heavily on the underlying data store. The two specific cases that NServiceBus relies on the underling data store are [concurrent access to non-existing saga instances](/nservicebus/sagas/concurrency.md#concurrent-access-to-non-existing-saga-instances) and [concurrent access to existing saga instances](/nservicebus/sagas/concurrency.md#concurrent-access-to-existing-saga-instances).


### Concurrent access to non-existing saga instances

The persister uses [unique indexes](https://docs.mongodb.com/manual/core/index-unique/) to ensure only one document can contain the unique data.


### Concurrent access to existing saga instances

The persister uses a document versioning scheme built on the [findAndModify](https://docs.mongodb.com/manual/reference/command/findAndModify/) command to atomically update the existing persisted data only if it has not been changed since it was retrieved. Since the update is atomic, it will ensure that only one update to a saga will succeed if there are multiple simultaneous updates made to it.