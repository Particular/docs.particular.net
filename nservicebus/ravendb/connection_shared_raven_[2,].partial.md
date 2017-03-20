

## External shared store for all persisters

The RavenDB `DocumentStore` instance can be provided directly to NServiceBus to use for all persisters. This enables sharing the same application database for NServiceBus data as well.

snippet: ravendb-persistence-external-store


## External store for a specific persister

An externally created `DocumentStore` instance can be used for a specific persister (e.g. timeouts) by using the following code:

snippet: ravendb-persistence-specific-external-store


## Shared store for all persisters defined via connection string

Instead of connecting to a database on the local server, a connection string can be provided via configuration. The runtime will look for a connection string named `NServiceBus/Persistence/RavenDB` or `NServiceBus/Persistence`.

snippet: shared-document-store-via-connection-string

RavenDB connection strings can include the database name and other parameters as well. See [How to set up a connection string](https://ravendb.net/docs/article-page/3.0/csharp/client-api/setting-up-connection-string#Format) in the RavenDB documentation for more details. The runtime will use the connection string to create a shared `DocumentStore` instance for all persisters.


## Shared store for all persisters defined via connection parameters

Rather than specifying connection information in configuration, connection details can be established via an instance of `ConnectionParameters` that allows specifying Url, DatabaseName and the ApiKey for the RavenDB instance for usage in all the persisters. The runtime will use the parameters to create a shared `DocumentStore` instance for all persisters.

snippet: ravendb-persistence-external-connection-params


## Store defined via a connection string for a specific persister

One can configure a RavenDB connection string that is only applicable to a specific store:

snippet: specific-document-store-via-connection-string
