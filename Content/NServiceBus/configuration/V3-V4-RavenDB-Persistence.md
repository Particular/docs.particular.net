---
title: Configuration API RavenDB Persistence in V3 and V4
summary: Configuration API RavenDB Persistence in V3 and V4
tags:
- NServiceBus
- Fluent Configuration
- V3
- V4
---

Some NServiceBus features rely on persistence storage to work properly. Beginning with V3 the default persistence storage is RavenDB.

* `RavenPersistence()`: configures the endpoint to use RavenDB and expects to find a connection string in the endpoint configuration file, named `NServiceBus/Persistence`.
* `RavenPersistence( 
*  connectionString )`: configures the endpoint to use RavenDB using the supplied RavenDB connection string.
* `RavenPersistence( Func<string> connectionStringProvider )`: configures the endpoint to use RavenDB and invokes the supplied delegate to get a valid RavenDB connection string at runtime.
* `RavenPersistence( Func<string> connectionStringProvider, string dbName )`: configures the endpoint to use RavenDB, invokes the supplied delegate to get a valid RavenDB connection string at runtime, and expects the name of the database as the second parameter.
* `RavenPersistenceWithStore( IDocumentStore store )`: configures the endpoint to use RavenDB using the supplied IDocumentStore.
* `RavenSagaPersister()`: configures sagas to use RavenDB as storage.
* `RavenSubscriptionStorage()`: configures the subscription manager to use RavenDB as storage.

For a detailed explanation on how to connect to RavenDB, read the [Connecting to RavenDB from NServiceBus](/nservicebus/using-ravendb-in-nservicebus-connecting) article.