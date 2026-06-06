---
title: Connection Options
component: raven
related:
 - samples/ravendb
redirects:
 - nservicebus/using-ravendb-in-nservicebus-connecting
 - nservicebus/ravendb/connecting
 - nservicebus/ravendb/connection
reviewed: 2026-06-03
---

## Connection to the RavenDB server

The following sections outline various ways to connect to the RavenDB server.

### External shared store for all persisters

This is the recommended method to connect to the database.
The RavenDB `DocumentStore` instance can be provided directly to NServiceBus to use for all persisters. This enables sharing the same application database for NServiceBus data as well.

snippet: ravendb-persistence-external-store

### External store for a specific persister

An externally created `DocumentStore` instance can be used for a specific persister (e.g. timeouts) by using the following code:

snippet: ravendb-persistence-specific-external-store

### External shared store at initialization

To use an external `DocumentStore`, but defer its creation until NServiceBus initializes, a custom factory delegate can be provided which will allow the `DocumentStore` to be created with access to the settings and the dependency injection container. This gives the ability to configure the `DocumentStore` based on conventions derived from endpoint data present in the settings object. For example, the `DocumentStore` instance can be configured to use the [Endpoint Name](/nservicebus/endpoints/specify-endpoint-name.md) as its database name by accessing `readOnlySettings.EndpointName()`.

snippet: ravendb-persistence-create-store-by-func

include: raven-dispose-warning

### External store at initialization for a specific persister

A `DocumentStore` can be created at initialization time, with access to endpoint settings and the dependency injection container, for usage in a specific persister (e.g. timeouts) by using the following code:

snippet: ravendb-persistence-specific-create-store-by-func

include: raven-dispose-warning

### Default

If the connection is not specified by any other means, then a `DocumentStore` is created that connects to `http://localhost:8080` and uses the endpoint name as its database name. This default connection is used for all the persisters.

## Database used

> [!NOTE]
> The database must be created before using RavenDB persistence.

After connecting to a RavenDB server, NServiceBus decides which database to use. Unless it finds a default database specified in the connection string, NServiceBus uses the endpoint name as the database name. This means that if the endpoint is named `MyServer`, the database name will be `MyServer`. Each endpoint will have a separate database unless explicitly overridden via the connection string.

See also [How to specify endpoint name](/nservicebus/endpoints/specify-endpoint-name.md).
