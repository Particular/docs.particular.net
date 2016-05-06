---
title: RavenDB Persistence Upgrade from 3 to 4
summary: Instructions on how to upgrade NServiceBus.RavenDB 3 to 4
tags:
 - upgrade
 - migration
 - raven
related:
 - nservicebus/upgrades/5to6
redirects:
 - nservicebus/ravendb/upgrades/3to4
---

As part of this update [NServiceBus Version 6](/nservicebus/upgrades/5to6.md) will be required.


## Namespace changes

Namespaces for public types have been consolidated to make customizations more discoverable. The primary namespaces are `NServiceBus` for customization options that need to be discoverable, and `NServiceBus.Persistence.RavenDB` for items off the beaten path. If the NServiceBus.RavenDB NuGet package is installed, a single `using NServiceBus` directive should be sufficient to find all necessary options.

As part of this move, the following classes were moved to different namespaces:

* `NServiceBus.Persistence.RavenDBPersistence` to the `NServiceBus` namespace
* `NServiceBus.RavenDB.Outbox.RavenDBOutboxExtensions` to the `NServiceBus` namespace
* `NServiceBus.RavenDB.ConnectionParameters` to the `NServiceBus.Persistence.RavenDB` namespace


## Configuring a shared session

Configuring a shared raven session now requires a `Func<IAsyncDocumentSession>` ([IAsyncDocumentSession](http://ravendb.net/docs/search/latest/csharp?searchTerm=IAsyncDocumentSession)) instead of a `Func<IDocumentSession>` [IDocumentSession](http://ravendb.net/docs/search/latest/csharp?searchTerm=IDocumentSession).

snippet:3to4-ravensharedsession


## ISessionProvider is obsolete

In Version 3 of NServiceBus.RavenDB an `ISessionProvider` was available for dependency injection. The new method of accessing the raven session is the `SynchronizedStorageSession`.

snippet:3to4-acccessingravenfromhandler


### Session is available regardless of features enabled

In Version 3, the `RavenStorageSession` was only registered if at least one out of [Outbox](/nservicebus/outbox/) and [Sagas](/nservicebus/sagas/) were enabled. There are possible use cases for using the NServiceBus wrapped RavenDB session so the prerequisites have been removed.
