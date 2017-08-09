---
title: RavenDB Persistence
component: raven
versions: '[2,)'
tags:
 - Persistence
related:
 - samples/ravendb
reviewed: 2016-10-04
redirects:
 - nservicebus/ravendb
---

Uses the [RavenDB document database](https://ravendb.net/) for storage.


## Connection options for RavenDB

There are a variety of options for configuring the connection to a RavenDB Server. See [RavenDB Connection Options](connection.md) for more details.


## Shared session

NServiceBus supports sharing the same RavenDB document session between Saga persistence, Outbox persistence, and business data, so that a single transaction can be used to persist the data for all three concerns atomically.

Shared session is only applicable to Saga and Outbox storage. It can be configured via

snippet: ravendb-persistence-shared-session-for-sagas

This optionally allows customization of the document session that is created for Saga, Outbox, and handler logic to share.

The session that is created is then made available.

include: raven-dispose-warning


### Using in a Handler

snippet: ravendb-persistence-shared-session-for-handler


### Using in a Saga

include: saga-business-data-access

snippet: ravendb-persistence-shared-session-for-saga

partial: unsafereads


## Distributed Transaction Coordinator settings

The RavenDB client requires a unique Guid to identify it to the Distributed Transaction Coordinator, and a method of storing DTC transaction recovery information in the case of process faults. By default, NServiceBus uses `IsolatedStorageTransactionRecoveryStorage` as its transaction recovery storage. Under certain high-load situations, this has been known to result in a `TransactionAbortedException` or `IsolatedStorageException`.

In order to set DTC settings that are safe for production use, refer to [Setting RavenDB DTC settings manually](manual-dtc-settings.md).


## Subscription persister and message versioning

The behavior of the RavenDB subscription persistence differs from other NServiceBus persisters in the way it handles versioning of message assemblies. It's important to understand this difference, especially when using a deployment solution that automatically increments assembly version numbers with each build.

To learn about message versioning as it relates to the RavenDB subscription persister, refer to [RavenDB subscription versioning](subscription-versioning.md).


## Viewing the data

Open a web browser and type the URL of the RavenDB server. This opens the [RavenDB Studio](https://ravendb.net/docs/search/latest/csharp?searchTerm=management-studio).
