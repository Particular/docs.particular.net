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


## NServiceBus 5 and higher

Starting with NServiceBus 5.0, RavenDB is no longer merged into the core. The RavenDB-backed persistence is available as a separate [NuGet package](https://www.nuget.org/packages/NServiceBus.RavenDB). This gives the ultimate freedom of evolution for both NServiceBus and RavenDB and allows users to upgrade both independently.


### Connection options for RavenDB

Following list outlines various ways to telling how to connect to the RavenDB server ordered from the highest to the lowest priority


#### Shared session

Shared session is only applicable to Saga and Outbox storage. It can be configured via

snippet:ravendb-persistence-shared-session-for-sagas

The session configured is then made available  user code via the `ISessionProvider` interface. e.g. in the handler:

snippet:ravendb-persistence-shared-session-for-sagas-handler


#### External store for a specific persister

One can pass an externally created `DocumentStore` instance for usage in a specific persister (e.g. timeouts) by using following code:

snippet:ravendb-persistence-specific-external-store


#### Store defined via a connection string for a specific persister

One can configure a RavenDB connection string that is only applicable to a specific store:

<!-- specific-document-store-via-connection-string -->


#### External shared store for all persisters

If for a given persister no method listed above works, NServiceBus looks for a shared externally-provided store. One can provide such a store via:

snippet:ravendb-persistence-external-store


#### Shared store for all persisters defined via connection parameters

One can pass an instance of `ConnectionParameters` that allows to specify Url, DatabaseName and the ApiKey of RavenDB instance for usage in all the persisters. The runtime will use the parameters to create a shared `DocumentStore`.

snippet:ravendb-persistence-external-connection-params


#### Shared store for all persisters defined via connection string

If nothing above succeeded, the runtime looks for shared store connection strings in the order below:

snippet:shared-document-store-via-connection-string

If a connection string is found, a corresponding shared `DocumentStore` is created.


#### Default

As a last resort, a `DocumentStore` that connects to `http://localhost:8080` is created and used for all the persisters.


### Other configuration options


#### Saga correlation

NOTE: As of Version 6 of NServiceBus, all correlated properties are unique by default so there is no longer a configuration setting. 

One of the limitations of the RavenDB persistence is support for only one `[Unique]` property (a saga property which value is guaranteed to be unique across all sagas of this type). Because of that limitation advanced user can turn off the validation that ensures sagas are only being found by unique properties:

snippet:ravendb-persistence-stale-sagas

DANGER: This is a potentially dangerous feature that can result in multiple instances of saga being created instead of one in cases of high contention.


#### Transaction recovery storage

Prior to NServiceBus.RavenDB 3.1.0, NServiceBus chose  `IsolatedStorageTransactionRecoveryStorage` as its transaction recovery storage for RavenDB, which is a setting necessary for RavenDB's implementation of distributed transactions. Later, RavenDB discovered that the use of Isolated Storage for this task was unstable in certain situations, resulting in users [receiving a TransactionAbortedException or IsolatedStorageException](https://groups.google.com/forum/#!msg/ravendb/4UHajkua5Q8/ZbsNYv6XkFoJ).

From NServiceBus.RavenDB 3.1.0 onwards, NServiceBus utilizes `LocalDirectoryTransactionRecoveryStorage` with a storage location inside `%LOCALAPPDATA%`.  It is strongly advised to upgrade to at least NServiceBus.RavenDB 3.1.0.

If experiencing one of these issues and an upgrade to 3.1.0 is not possible, the default TransactionRecoveryStorage can be changed as shown in the following example. Note that a version for NServiceBus 6+ is not shown as versions of NServiceBus.RavenDB compatible with NServiceBus 6+ do not contain the issue.

snippet:ConfiguringTransactionRecoveryStorage


## Previous releases

Beginning with NServiceBus 3.0, including all 3.x and 4.x versions, RavenDB is the default mechanisms for NServiceBus to persist its time-out, saga and subscription information.

To tell NServiceBus to use RavenDB for persistence is as easy as calling `Configure.RavenPersistence()`. This is the default configuration and it uses these conventions:

-   If no master node is configured it assumes that a RavenDB server is running at `http://localhost:8080`, the default URL for RavenDB.
-   If a master node is configured, the URL is: `http://{masternode}/:8080`.
-   If a connection string named "NServiceBus/Persistence" is found, the value of the `connectionString` attribute is used.

This gives you full control over which RavenDB server your endpoint uses.

If NServiceBus detects that any RavenDB related storage is used for sagas, subscriptions, timeouts, etc., if automatically configures it for you. So in essence there is no need for you to explicitly configure RavenDB unless you want to override the defaults.


### Overriding the defaults

In some situations the default behavior might not be right for you:

-   You want to use your own connection string. If you're using RavenDB for your own data as well you might want to share the connection string. Use the `Configure.RavenPersistence(string connectionStringName)` signature to tell NServiceBus to connect to the server specified in that string. The default connection string for RavenDB is "RavenDB".
-   You want to specify a explicit database name. To control the database name in code instead of via the configuration, use the `Configure.RavenPersistence(string connectionStringName, string databaseName)` signature. This can be useful in a multi-tenant scenario.


### Can I use the IDocumentStore used by NServiceBus for my own data?

No, the RavenDB client is merged and internalized into the NServiceBus assemblies, so to use Raven for your own purposes, reference the Raven client and set up your own document store.

NOTE: In NServiceBus 4.x RavenDB is not ilmerged any more. It is embedded instead, using [https://github.com/Fody/Costura\#readme](https://github.com/Fody/Costura#readme).

The embedding enables client updates (but may require binding redirects). It also allows passing your own `DocumentStore`, thereby providing full configuration flexibility.


## Which database is used?

After connecting to a RavenDB server, decide which actual database to use. Unless NServiceBus finds a default database specified in the connection string, NServiceBus uses the endpoint name as the database name. So if your endpoint is named `MyServer`, the database name is `MyServer`. Each endpoint has a separate database unless you explicitly override it using the connection string. RavenDB automatically creates the database if it doesn't already exist.

See also [How to specify your input queue name](/nservicebus/messaging/specify-input-queue-name.md).


## How do I look at the data?

Open a web browser and type the URL of the RavenDB server. This opens the [RavenDB Studio](http://ravendb.net/docs/search/latest/csharp?searchTerm=management-studio).
