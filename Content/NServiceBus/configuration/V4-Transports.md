---
title: Configuration API Transports in V4
summary: Configuration API Transports in V4
tags:
- NServiceBus
- Fluent Configuration
- V4
---

In V4, given the requirement to support multiple transports, call the `UseTransport()` method of the `Configure` instance:

* `UseTransport<TTransport>( "connection string (optional)" )`: the generic overload of the UseTransport method can be invoked using a transport class as generic parameter and optionally passing in a transport connection string.
* `UseTransport( Type transportType, "connection string (optional)" )`: the non-generic overload of the `UseTransport()` method accepts a `Type` instance that is the type of transport class and optionally the transport connection string.

The list of the built-in supported transport is available in the [NServiceBus Connection String Samples](/nservicebus/connection-strings-samples) article.