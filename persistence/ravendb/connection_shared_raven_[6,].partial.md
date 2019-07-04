

## External shared store for all persisters

The RavenDB `DocumentStore` instance can be provided directly to NServiceBus to use for all persisters. This enables sharing the same application database for NServiceBus data as well.

snippet: ravendb-persistence-external-store


## External store for a specific persister

An externally created `DocumentStore` instance can be used for a specific persister (e.g. timeouts) by using the following code:

snippet: ravendb-persistence-specific-external-store