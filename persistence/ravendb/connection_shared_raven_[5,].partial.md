

## External shared store for all persisters

The RavenDB `DocumentStore` instance can be provided directly to NServiceBus to use for all persisters. This enables sharing the same application database for NServiceBus data as well.

snippet: ravendb-persistence-external-store


## External store for a specific persister

An externally created `DocumentStore` instance can be used for a specific persister (e.g. timeouts) by using the following code:

snippet: ravendb-persistence-specific-external-store


## Shared store for all persisters defined via connection parameters

Rather than specifying connection information in configuration, connection details can be established via an instance of `ConnectionParameters` that allows specifying Url, DatabaseName and the ApiKey for the RavenDB instance for usage in all the persisters. The runtime will use the parameters to create a shared `DocumentStore` instance for all persisters.

snippet: ravendb-persistence-external-connection-params