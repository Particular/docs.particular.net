---
title: RavenDB
summary: RavenDB persister documentation
tags:
- RavenDB
- Persistence
related:
- samples/ravendb
redirects:
 - nservicebus/using-ravendb-in-nservicebus-connecting
 - nservicebus/ravendb/connecting
---

Uses the [RavenDB document database](http://ravendb.net/) for storage.


## NServiceBus 5 and above

RavenDB is no longer merged into the core. The RavenDB-backed persistence is available as a separate [NuGet package](https://www.nuget.org/packages/NServiceBus.RavenDB). This allows NServiceBus and RavenDB to be upgraded independently.


### Connection options for RavenDB

The following sections outline various ways to connect to the RavenDB server, starting with the most basic.


#### Default

By default, a `DocumentStore` is created that connects to `http://localhost:8080` and uses the endpoint name as its database name. This default connection is used for all the persisters.


#### Shared store for all persisters defined via connection string

Instead of connecting to a database on the local server, a connection string can be provided via configuration. The runtime will look for a connection string named `NServiceBus/Persistence/RavenDB` or `NServiceBus/Persistence`.

snippet:shared-document-store-via-connection-string

RavenDB connection strings can include the database name and other parameters as well. See [How to set up a connection string](https://ravendb.net/docs/article-page/3.0/csharp/client-api/setting-up-connection-string#Format) in the RavenDB documentation for more details. The runtime will use the the connection string to create a shared `DocumentStore` instance for all persisters.


#### Shared store for all persisters defined via connection parameters

Rather than specifying connection information in configuration, connection details can be established via an instance of `ConnectionParameters` that allows specifying Url, DatabaseName and the ApiKey for the RavenDB instance for usage in all the persisters. The runtime will use the parameters to create a shared `DocumentStore` instance for all persisters.

snippet:ravendb-persistence-external-connection-params


#### External shared store for all persisters

If NServiceBus needs to use the same `DocumentStore` instance used elsewhere in an application, this instance can be provided directly to NServiceBus to use for all persisters.

snippet:ravendb-persistence-external-store


#### Store defined via a connection string for a specific persister

One can configure a RavenDB connection string that is only applicable to a specific store:

snippet:specific-document-store-via-connection-string


#### External store for a specific persister

An externally created `DocumentStore` instance can be passed to NServiceBus for usage in a specific persister (e.g. timeouts) by using the following code:

snippet:ravendb-persistence-specific-external-store


### Other configuration options


#### Shared session

NServiceBus supports sharing the same RavenDB document session between Saga persistence, Outbox persistence, and business data, so that a single transaction can be used to persist the data for all three concerns atomically.

Shared session is only applicable to Saga and Outbox storage. It can be configured via

snippet:ravendb-persistence-shared-session-for-sagas

This optionally allows customization of the document session that is created for Saga, Outbox, and handler logic to share.

The session that is created is then made available to handler logic, although the method differs based on NServiceBus version:

snippet:ravendb-persistence-shared-session-for-sagas-handler


#### Saga correlation

NOTE: As of Version 6 of NServiceBus, all correlated properties are unique by default so there is no longer a configuration setting. 

One of the limitations of the RavenDB persistence is support for only one `[Unique]` property (a saga property which value is guaranteed to be unique across all sagas of this type). Because of that limitation advanced user can turn off the validation that ensures sagas are only being found by unique properties:

snippet:ravendb-persistence-stale-sagas

DANGER: This is a potentially dangerous feature that can result in multiple instances of saga being created instead of one in cases of high contention.


#### Transaction recovery storage

The RavenDB client requires a method of storing DTC transaction recovery information in the case of process faults. The handling of transaction recovery storage by NServiceBus.RavenDB differs by version.


##### NServiceBus.RavenDB 3.1 and above

As of 3.1.0, NServiceBus uses `LocalDirectoryTransactionRecoveryStorage` with a storage location inside `%LOCALAPPDATA%`. It is not necessary to modify this default value.


##### NServiceBus.RavenDB 3.0.x and below

In these versions of NServiceBus, NServiceBus uses `IsolatedStorageTransactionRecoveryStorage` as its transaction recovery storage, which has been proven to be unstable in certain situations, sometimes resulting in a [TransactionAbortedException or IsolatedStorageException](https://groups.google.com/forum/#!msg/ravendb/4UHajkua5Q8/ZbsNYv6XkFoJ).

If experiencing one of these issues and an upgrade to 3.1.0 or later is not possible, the default `TransactionRecoveryStorage` can be changed as shown in the following example.

snippet:ConfiguringTransactionRecoveryStorage


## NServiceBus 3 and NServiceBus 4

For all NServiceBus 3.x and 4.x versions, RavenDB is the default mechanism for NServiceBus to persist its timeout, saga and subscription data.

Configuring NServiceBus to use RavenDB for persistence can be accomplished by calling `Configure.RavenPersistence()`, however as this is the default configuration this call is not required.

RavenDB persistence for NServiceBus 3/4 uses these conventions:

 * If no master node is configured it assumes that a RavenDB server is running at `http://localhost:8080`, the default URL for RavenDB.
 * If a master node is configured, the URL is: `http://{masternode}/:8080`.
 * If a connection string named "NServiceBus/Persistence" is found, the value of the `connectionString` attribute is used.

If NServiceBus detects that any RavenDB related storage is used for sagas, subscriptions, timeouts, etc., if automatically configures it for you. There is no need to explicitly configure RavenDB unless it is necessary to override the defaults.


### Overriding the defaults

In some situations the default behavior might not be right for you:

-   You want to use your own connection string. If you're using RavenDB for your own data as well you might want to share the connection string. Use the `Configure.RavenPersistence(string connectionStringName)` signature to tell NServiceBus to connect to the server specified in that string. The default connection string for RavenDB is "RavenDB".
-   You want to specify a explicit database name. To control the database name in code instead of via the configuration, use the `Configure.RavenPersistence(string connectionStringName, string databaseName)` signature. This can be useful in a multi-tenant scenario.


### Can IDocumentStore used by NServiceBus for business data?

No, the RavenDB client is merged and internalized into the NServiceBus assemblies, so to use Raven for your own purposes, reference the Raven client and set up your own document store.

NOTE: In NServiceBus 4.x RavenDB is not ilmerged any more. It is embedded instead, using [https://github.com/Fody/Costura\#readme](https://github.com/Fody/Costura#readme).

The embedding enables client updates (but may require binding redirects). It also allows passing your own `DocumentStore`, thereby providing full configuration flexibility.


## Which database is used?

After connecting to a RavenDB server, decide which actual database to use. Unless NServiceBus finds a default database specified in the connection string, NServiceBus uses the endpoint name as the database name. So if the endpoint is named `MyServer`, the database name is `MyServer`. Each endpoint has a separate database unless you explicitly override it using the connection string. RavenDB automatically creates the database if it doesn't already exist.

See also [How to specify an input queue name](/nservicebus/messaging/specify-input-queue-name.md).


## How do I look at the data?

Open a web browser and type the URL of the RavenDB server. This opens the [RavenDB Studio](http://ravendb.net/docs/search/latest/csharp?searchTerm=management-studio).