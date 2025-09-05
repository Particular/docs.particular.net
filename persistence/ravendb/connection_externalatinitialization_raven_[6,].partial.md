## External shared store at initialization

To use an external `DocumentStore`, but defer its creation until NServiceBus initializes, a custom factory delegate can be provided which will allow the `DocumentStore` to be created with access to the settings and the dependency injection container. This gives the ability to configure the document store based on conventions derived from endpoint data present in the settings object. For example, the `DocumentStore` instance can be configured to use the [Endpoint Name](/nservicebus/endpoints/specify-endpoint-name.md) as its database name by accessing `readOnlySettings.EndpointName()`.

snippet: ravendb-persistence-create-store-by-func


## External store at initialization for a specific persister

A `DocumentStore` can be created at initialization time, with access to endpoint settings and the dependency injection container, for usage in a specific persister (e.g. timeouts) by using the following code:

snippet: ravendb-persistence-specific-create-store-by-func