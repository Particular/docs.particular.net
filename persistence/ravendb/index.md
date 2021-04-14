---
title: RavenDB Persistence
component: raven
versions: '[2,)'
related:
 - samples/ravendb
 - samples/multi-tenant/ravendb
reviewed: 2019-06-10
redirects:
 - nservicebus/ravendb
 - persistence/ravendb/licensing
 - nservicebus/ravendb/licensing
---

include: dtc-warning

include: cluster-configuration-warning

Uses the [RavenDB document database](https://ravendb.net/) for storage.


## Persistence at a glance

partial: glance

## RavenDB versions

Specific versions of RavenDB Persistence are tied to a major version of NServiceBus and also designed to work with a specific version of the RavenDB client library. When releasing a new major version of NServiceBus, the corresponding RavenDB Persistence release will use the last supported version of RavenDB, so that it is never necessary to upgrade both NServiceBus and RavenDB at the same time.

| NServiceBus | RavenDB Persistence | RavenDB Client | Platform    |
|:-----------:|:-------------------:|:--------------:|:-----------:|
|     7.x     |        6.x          |       4.2      | .NET 4.7.2 / .NET Core 2.1  |
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

NServiceBus does not support multi-master RavenDB cluster configurations. Only fail-over clustering modes are supported. Consistency cannot be guaranteed in multi-master configurations and NServiceBus cannot do automated conflict resolution for conflicts in multi-master configurations.

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

partial: subscription-versioning


## Viewing the data

Open a web browser and type the URL of the RavenDB server. This opens the [RavenDB Studio](https://ravendb.net/docs/search/latest/csharp?searchTerm=management-studio).


## Migrating timeouts

Timeouts can be migrated to the native-delay delivery implementation with the [migration tool](/nservicebus/tools/migrate-to-native-delivery.md).
