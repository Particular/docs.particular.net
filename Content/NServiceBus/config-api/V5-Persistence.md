---
title: Configuration API Persistence in V5
summary: Configuration API Persistence in V5
tags:
- NServiceBus
- BusConfiguration
- V5
- Persistence
---

Some NServiceBus features rely on persistence storage to work properly. Until V4 the default persistence storage was RavenDB, now RavenDB is not part of the core anymore and has been externalized as a separate [package](http://www.nuget.org/packages/NServiceBus.RavenDB/).

To define the persistence engine to use the `UsePersistence()` method must be called on the `BusConfiguration` instance. The `UsePersistence()` method returns a `PersitenceExtensions<TPersistence>` instance that allows the caller to define, via the `For()` method, for which features the given persistence storage should be used:

* `Storage.Timeouts`: Storage for timeouts;
* `Storage.Subscriptions`: Storage for subscriptions;
* `Storage.Sagas`: Storage for sagas;
* `Storage.GatewayDeduplication`: Storage for gateway deduplication;
* `Storage.Outbox`: Storage for the outbox;

If the `For()` method is not called NServiceBus assumes that the given persistence should be used for all the features that requires persistence.