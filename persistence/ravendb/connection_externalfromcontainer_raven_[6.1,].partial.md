## External store from container

To use an external `DocumentStore` resolved from a container, a `Func<IBuilder, IDocumentStore>` can be provided:

snippet: ravendb-persistence-resolve-from-container


## External store from container for a specific persister

A `DocumentStore` for usage in a specific persister (e.g. timeouts) can be resolved from the container using:

snippet: ravendb-persistence-specific-resolve-from-container