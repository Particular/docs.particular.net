---
title: Configuration API RavenDB Persistence in V5
summary: Configuration API RavenDB Persistence in V5
tags:
- NServiceBus
- BusConfiguration
- V5
- Persistence
---

To configure the persitence engine to use RavenDB as the persitence storage call the `UsePersistence<RavenDBPersitence()`, or the `UsePersistence( typeof( RavenDBPersitence ) )`, method of the `BusConfiguration` class. The `NServiceBus.RavenDB` persistence package adds some behaviors to the default `PersitenceExtensions<TPersistence>` instance:

* `SetDefaultDocumentStore`: sets the default RavenDB document store to use as default for storage;
* `UseDocumentStoreForGatewayDeduplication`: sets the default RavenDB document store for gateway deduplication;
* `UseDocumentStoreForSagas`: sets the default RavenDB document store for saga storage;
* `UseDocumentStoreForSubscriptions`: sets the default RavenDB document store for subscriptions storage;
* `UseDocumentStoreForTimeouts`: sets the default RavenDB document store for timeouts storage;
* `DoNotSetupDatabasePermissions`: instructs NServiceBus to not try to setup database permissions on the current storage at startup;
* `AllowStaleSagaReads`: allows the saga storage to retrieve sagas even if the saga query returns stale data;
* `UseSharedSession`: setups a shared RavenDB session that can be used to retrieve data from the RavenDB storage;

For a detailed explanation on how to connect to RavenDB, read the [Connecting to RavenDB from NServiceBus](/nservicebus/using-ravendb-in-nservicebus-connecting) article.