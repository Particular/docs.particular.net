---
title: Connection Options
component: raven
tags:
 - Persistence
related:
 - samples/ravendb
redirects:
 - nservicebus/using-ravendb-in-nservicebus-connecting
 - nservicebus/ravendb/connecting
 - nservicebus/ravendb/connection
reviewed: 2017-08-24
---

include: dtc-warning

The following sections outline various ways to connect to the RavenDB server. Specifying an external shared store (providing a fully configured RavenDB `DocumentStore` instance) is preferred so that the [RavenDB DTC settings can be configured](manual-dtc-settings.md) for optimal data safety.

include: raven-dispose-warning

partial: externalatinitialization


partial: shared


## Default

By default, a `DocumentStore` is created that connects to `http://localhost:8080` and uses the endpoint name as its database name. This default connection is used for all the persisters.


## Database used

After connecting to a RavenDB server, decide which database to use. Unless NServiceBus finds a default database specified in the connection string, NServiceBus uses the endpoint name as the database name. So if the endpoint is named `MyServer`, the database name is `MyServer`. Each endpoint has a separate database unless explicitly overridden via the connection string. RavenDB automatically creates the database if it doesn't already exist.

See also [How to specify endpoint name](/nservicebus/endpoints/specify-endpoint-name.md).


partial: caveats
