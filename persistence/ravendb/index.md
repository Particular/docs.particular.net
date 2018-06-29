---
title: RavenDB Persistence
component: raven
versions: '[2,)'
tags:
 - Persistence
related:
 - samples/ravendb
reviewed: 2018-02-19
redirects:
 - nservicebus/ravendb
---

include: dtc-warning

Uses the [RavenDB document database](https://ravendb.net/) for storage.


## RavenDB versions

Specific versions of RavenDB Persistence are tied to a major version of NServiceBus and also designed to work with a specific version of the RavenDB client library. When releasing a new major version of NServiceBus, the corresponding RavenDB Persistence release will use the last supported version of RavenDB, so that it is never necessary to upgrade both NServiceBus and RavenDB at the same time.

| NServiceBus | RavenDB Persistence | RavenDB Client | Platform    |
|:-----------:|:-------------------:|:--------------:|:-----------:|
|     7.x     |  *Not Yet Released* |       4.0      | .NET 4.6+ / .NET Core 2.0  |
|     7.x     |        5.0.x        |       3.5      | .NET 4.5.2 / .NET Core 2.0  |
|     6.x     |        4.2.x        |       3.5      | .NET 4.5.2  |
|     6.x     |        4.0.x        |       3.0      | .NET 4.5.2  |
|     5.x     |        3.2.x        |       3.5      | .NET 4.5.2  |
|     5.x     |        3.0.x        |       3.0      | .NET 4.5.2  |
|     5.x     |         2.x         |       2.5      | .NET 4.5.2  |

See the [NServiceBus Packages Supported Versions](/nservicebus/upgrades/supported-versions.md#persistence-packages-nservicebus-ravendb) to see the support details for each version of RavenDB Persistence.


## Connection options for RavenDB

There are a variety of options for configuring the connection to a RavenDB Server. See [RavenDB Connection Options](connection.md) for more details.

## Supported clustering configurations

NServiceBus does not support multi-master RavenDB cluster configurations. Only fail-over clustering modes are supported. Consistency cannot be guaranteerd in multi-master configurations. NServiceBus cannot do automated conflict resolution for conflicts in multi-master   configurations.

## Shared session

NServiceBus supports sharing the same RavenDB document session between Saga persistence, Outbox persistence, and business data, so that a single persistence transaction can be used to persist the data for all three concerns atomically. Shared sessions are automatically configured when an endpoint has enabled the [Outbox feature](/nservicebus/outbox/) or contains [sagas](/nservicebus/sagas/).

To use the shared session in a message handler:

snippet: ravendb-persistence-shared-session-for-handler

Although additional database operations inside a saga handler are not recommended (see warning below) the shared session can also be accessed from a saga handler:

include: saga-business-data-access

snippet: ravendb-persistence-shared-session-for-saga


## Customizing the IDocumentSession

partial: shared-session-api-incompatible-with-outbox

The creation of the RavenDB `IDocumentSession` instance used by NServiceBus and made available as the [shared session](#shared-session) can be customized as shown in the following snippet. Despite the name of the method, this option *does not enable the shared session* but only affects the customization of that session.

snippet: ravendb-persistence-customize-document-session

include: raven-dispose-warning

partial: multitenant

partial: unsafereads


## Subscription persister and message versioning

The behavior of the RavenDB subscription persistence differs from other NServiceBus persisters in the way it handles versioning of message assemblies. It's important to understand this difference, especially when using a deployment solution that automatically increments assembly version numbers with each build.

To learn about message versioning as it relates to the RavenDB subscription persister, refer to [RavenDB subscription versioning](subscription-versioning.md).


## Viewing the data

Open a web browser and type the URL of the RavenDB server. This opens the [RavenDB Studio](https://ravendb.net/docs/search/latest/csharp?searchTerm=management-studio).
