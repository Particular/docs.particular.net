---
title: Configuration API entry point in V5
summary: Configuration API entry point in V5
tags:
- NServiceBus
- BusConfiguration
- V5
---

### Configuration API

NServiceBus V5 introduces a new configuration API to overcome limitations of the [previous approach](config-api-V3-V4-entry-point). The new configuration engine is a two step configuration engine where at startup time a new configuration can be defined and then used to create an `IBus` instance that will rely on a set of settings built given the original configuration, the `IBus` runtime settings are `read-only` and can only be changed recreating the bus.

The major change introduced in V5 is that NServiceBus V5 endpoints can now host multiple bus instances running different configurations. 

#### Configuration Entry Point

The NServiceBus configuration entry point is the `BusConfiguration` class. In a self-hosting scenario we can manually create an instance of the `BusConfiguration` class, in a scenario where we are using the `NServiceBus.Host` hosting service a new instance is created by the hosting engine and will be given to the endpoint configuration class at startup time.   

If we need to specify which assemblies should be scanned at startup time we can rely on the `AssembliesToScan()` method; in order to specify which types should be scanned we can rely on the `TypesToScan()` method.

It is also possible to define a custom probing directory to override the default one, that is the one where the process is running from. In order to change the probing directory call the `ScanAssembliesInDirectory` method.

*NOTE*: The supplied assemblies/types must also contain all the NServiceBus assemblies or types;