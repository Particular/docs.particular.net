---
title: RavenDB Persistence Upgrade from 3 to 4
summary: Instructions on how to upgrade NServiceBus.RavenDB 3 to 4
component: Raven
related:
 - nservicebus/upgrades/5to6
redirects:
 - nservicebus/ravendb/upgrades/3to4
 - nservicebus/upgrades/ravendb-3to4
reviewed: 2019-11-11
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---

include: dtc-warning

include: cluster-configuration-warning

As part of this update, [NServiceBus Version 6](/nservicebus/upgrades/5to6/) will be required.


## Migrate saga data if using AllowStaleSagaReads

Due to [changes in how NServiceBus Version 6 handles saga correlation properties](/nservicebus/upgrades/5to6/handlers-and-sagas.md#saga-api-changes-unique-attribute-no-longer-needed), solutions that previously used the `AllowStaleSagaReads()` option in RavenDB Persistence will not work properly and need to be migrated during upgrade.

DANGER: Failure to migrate saga data when using the `AllowStaleSagaReads()` option will result in NServiceBus being unable to locate saga data, which will cause it to create duplicate saga data erroneously.

To enable efficient saga loading in one server round-trip, RavenDB persistence will use a _pointer document_ to load the saga data document by the correlation property in an atomic and consistent manner, without using RavenDB indexes that are (by design) not updated atomically with the document store.

When using the `AllowStaleSagaReads()` option in previous versions, which was sometimes used to support correlating saga data on multiple properties, pointer documents were not used and RavenDB indexes (which might be stale) were used instead.

Starting in NServiceBus Version 6, only one correlation id is supported, and the `AllowStaleSagaReads()` option is deprecated. However, saga data stored using this option in previous versions will not have the unique identity pointer document, and will not be able to load. The result is that NServiceBus will not be able to find the saga data document. If the message handler is implemented in the saga as `IHandleMessages<T>`, then the message will be incorrectly discarded as belonging to a saga that has already completed. If the message handler is implemented in the saga as `IAmStartedByMessages<T>`. then a new saga data will (incorrectly) be created, leading to duplicate saga data documents and incorrect business execution.

When upgrading, if the `AllowStaleSagaReads` option is in use, contact [support@particular.net](mailto:support@particular.net) for assistance in identifying the scope of the problem and migration of data.


## Namespace changes

Namespaces for public types have been consolidated to make customizations more discoverable. The primary namespaces are `NServiceBus` for customization options that need to be discoverable, and `NServiceBus.Persistence.RavenDB` for advanced APIs. A single `using NServiceBus` directive should be sufficient to find all necessary options.

As part of this move, the following classes were moved to different namespaces:

 * `NServiceBus.Persistence.RavenDBPersistence` to the `NServiceBus` namespace.
 * `NServiceBus.RavenDB.Outbox.RavenDBOutboxExtensions` to the `NServiceBus` namespace.
 * `NServiceBus.RavenDB.ConnectionParameters` to the `NServiceBus.Persistence.RavenDB` namespace.


## Use of RavenDB Async API

NServiceBus now uses the asynchronous RavenDB API for all operations. If sharing the session between NServiceBus and handler code is required, then handler code will need to be adjusted to utilize the asynchronous RavenDB API as well.

Previously, the API exposed an [`IDocumentSession`](https://ravendb.net/docs/search/latest/csharp?searchTerm=IDocumentSession), but now exposes [`IAsyncDocumentSession`](https://ravendb.net/docs/search/latest/csharp?searchTerm=IAsyncDocumentSession) instead, which contains the same operations using a Task-based API.


## Configuring a shared session

Configuring a shared raven session now requires a `Func<IAsyncDocumentSession>` instead of a `Func<IDocumentSession>`.

snippet: 3to4-ravensharedsession


## ISessionProvider is obsolete

In Version 3 of NServiceBus.RavenDB, an `ISessionProvider` was available for dependency injection. The new method of accessing the raven session is the `SynchronizedStorageSession`.

snippet: 3to4-acccessingravenfromhandler


### Session is available regardless of features enabled

In Version 3, the `RavenStorageSession` was only registered if at least one out of [Outbox](/nservicebus/outbox/) and [Sagas](/nservicebus/sagas/) were enabled. There are possible use cases for using the NServiceBus wrapped RavenDB session, so the prerequisites have been removed.
