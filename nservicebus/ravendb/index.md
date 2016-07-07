---
title: RavenDB Persistence
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

When using NServiceBus Versions 5 and above, the RavenDB-backed persistence is no longer ILMerged into the Core and is available as a separate [NuGet package](https://www.nuget.org/packages/NServiceBus.RavenDB), which allows NServiceBus and RavenDB to be upgraded independently. When using RavenDB persistence in NServiceBus endpoints Versions 4 and below, see the section titled [NServiceBus 3 and NServiceBus 4](/nservicebus/ravendb/#nservicebus-3-and-nservicebus-4) for more details.


### Connection options for RavenDB

The following sections outline various ways to connect to the RavenDB server. Specifying an external shared store (providing a fully configured RavenDB `DocumentStore` instance) is preferred so that the [RavenDB DTC settings can be configured](manual-dtc-settings.md) for optimal data safety.


#### External shared store for all persisters

The RavenDB `DocumentStore` instance can be provided directly to NServiceBus to use for all persisters. This enables sharing the same application database for NServiceBus data as well.

snippet:ravendb-persistence-external-store


#### External shared store at initialization

To use an external `DocumentStore`, but defer its creation until NServiceBus initializes, a `Func<ReadOnlySettings, IDocumentStore>` can be provided which will allow the `DocumentStore` to be created with access to the `ReadOnlySettings`. This gives the ability to configure the document store based on conventions derived from endpoint data present in the settings object. For example, the `DocumentStore` instance can be configured to use the [Endpoint Name](/nservicebus/endpoints/specify-endpoint-name.md) as its database name by accessing `readOnlySettings.EndpointName()`.

Versions: 4 and above.

snippet:ravendb-persistence-create-store-by-func


#### External store for a specific persister

An externally created `DocumentStore` instance can be passed to NServiceBus for usage in a specific persister (e.g. timeouts) by using the following code:

snippet:ravendb-persistence-specific-external-store


#### External store at initialization for a specific persister

A `DocumentStore` can be created when NServiceBus initializes, with access to endpoint settings found in `ReadOnlySettings`, for usage in a specific persister (e.g. timeouts) by using the following code:

Versions: 4 and above.

snippet:ravendb-persistence-specific-create-store-by-func


#### Shared store for all persisters defined via connection string

Instead of connecting to a database on the local server, a connection string can be provided via configuration. The runtime will look for a connection string named `NServiceBus/Persistence/RavenDB` or `NServiceBus/Persistence`.

snippet:shared-document-store-via-connection-string

RavenDB connection strings can include the database name and other parameters as well. See [How to set up a connection string](https://ravendb.net/docs/article-page/3.0/csharp/client-api/setting-up-connection-string#Format) in the RavenDB documentation for more details. The runtime will use the the connection string to create a shared `DocumentStore` instance for all persisters.


#### Shared store for all persisters defined via connection parameters

Rather than specifying connection information in configuration, connection details can be established via an instance of `ConnectionParameters` that allows specifying Url, DatabaseName and the ApiKey for the RavenDB instance for usage in all the persisters. The runtime will use the parameters to create a shared `DocumentStore` instance for all persisters.

snippet:ravendb-persistence-external-connection-params


#### Store defined via a connection string for a specific persister

One can configure a RavenDB connection string that is only applicable to a specific store:

snippet:specific-document-store-via-connection-string


#### Default

By default, a `DocumentStore` is created that connects to `http://localhost:8080` and uses the endpoint name as its database name. This default connection is used for all the persisters.


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


#### Distributed Transaction Coordinator settings

The RavenDB client requires a unique Guid to identify it to the Distributed Transaction Coordinator, and a method of storing DTC transaction recovery information in the case of process faults. By default, NServiceBus uses `IsolatedStorageTransactionRecoveryStorage` as its transaction recovery storage. Under certain high-load situations, this has been known to result in a `TransactionAbortedException` or `IsolatedStorageException`.

In order to set DTC settings that are safe for production use, refer to [Setting RavenDB DTC settings manually](manual-dtc-settings.md).

## NServiceBus 3 and NServiceBus 4

For all NServiceBus 3.x and 4.x versions, RavenDB is the default mechanism for NServiceBus to persist its timeout, saga and subscription data.

Configuring NServiceBus to use RavenDB for persistence can be accomplished by calling `Configure.RavenPersistence()`, however as this is the default configuration this call is not required.

RavenDB persistence for NServiceBus 3 to 4 uses these conventions:

 * If no master node is configured it assumes that a RavenDB server is running at `http://localhost:8080`, the default URL for RavenDB.
 * If a master node is configured, the URL is: `http://{masternode}/:8080`.
 * If a connection string named "NServiceBus/Persistence" is found, the value of the `connectionString` attribute is used.

If NServiceBus detects that any RavenDB related storage is used for sagas, subscriptions, timeouts, etc., it will automatically configure it. There is no need to explicitly configure RavenDB unless it is necessary to override the defaults.


### Overriding the defaults

In some situations the default behavior might not be desired:

 * When using RavenDB for business data as it may be necessary to share the connection string. Use the `Configure.RavenPersistence(string connectionStringName)` signature to tell NServiceBus to connect to the server specified in that string. The default connection string for RavenDB is `RavenDB`.
 * To control the database name in code, instead of via the configuration, use the `Configure.RavenPersistence(string connectionStringName, string databaseName)` signature. This can be useful in a multitenant scenario.


### Using the NServiceBus IDocumentStore for business data is not possible

The RavenDB client is merged and internalized into the NServiceBus assemblies. To use the Raven `IDocumentStore` for business data, reference the Raven client and set up a custom `IDocumentStore`.

NOTE: In NServiceBus 4.x RavenDB is not ILMerged any more. It is embedded instead, using [https://github.com/Fody/Costura\#readme](https://github.com/Fody/Costura#readme).

The embedding enables client updates (but may require binding redirects). It also allows passing a custom instance of `DocumentStore`, thereby providing full configuration flexibility.


## Database used

After connecting to a RavenDB server, decide which actual database to use. Unless NServiceBus finds a default database specified in the connection string, NServiceBus uses the endpoint name as the database name. So if the endpoint is named `MyServer`, the database name is `MyServer`. Each endpoint has a separate database unless explicitly overridden via the connection string. RavenDB automatically creates the database if it doesn't already exist.

See also [How to specify endpoint name](/nservicebus/endpoints/specify-endpoint-name.md).


## Viewing the data

Open a web browser and type the URL of the RavenDB server. This opens the [RavenDB Studio](http://ravendb.net/docs/search/latest/csharp?searchTerm=management-studio).