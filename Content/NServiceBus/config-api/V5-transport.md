---
title: Configuration API transport in V5
summary: Configuration API transport in V5
tags:
- NServiceBus
- BusConfiguration
- V5
---

The configuration of the NServiceBus transport is made via the `UseTransport()` method of the `BusConfiguration` class.

* `UseTransport<TTransport>()`: the generic overload of the UseTransport method can be invoked using a transport class as generic parameter.
* `UseTransport( Type transportType )`: the non-generic overload of the `UseTransport()` method accepts a `Type` instance that is the type of transport class.

In both cases the call to `UseTransport()` will return a `TransportExtensions` instance that allows the configuration of the transport connection string, via the `ConnectionString()` method, orthe transport connection string name via the `ConnectionStringName()` method.

The list of the built-in supported transport is available in the [NServiceBus Connection String Samples](connection-strings-samples) article.