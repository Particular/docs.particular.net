---
title: Configuration API entry point in V3 and V4
summary: Configuration API entry point in V3 and V4
tags:
- NServiceBus
- Fluent Configuration
- V3
- V4
---

### Fluent Configuration API

The NServiceBus configuration entry point is the `Configure` class and its static `With()` method. Each time you need to access an instance of the current configuration, use the static `Instance` property of the `Configuration` class. 

#### Configuration Entry Point

The `With()` method has several overloads, each resulting in the creation of a new configuration instance.

* `With()`: Initializes a new configuration, scanning all the assemblies found in the `bin` folder of the current application;
* `With(string probeDirectory)`: Initializes a new configuration, scanning all the assemblies found in the given `probeDirectory` folder;
* `With(params Assembly[] assemblies)`: Initializes a new configuration, scanning all the supplied assemblies; 

*NOTE*: The supplied assemblies must also contain the NServiceBus binaries;

* `With(IEnumerable<Type> typesToScan)`: Initializes a new configuration, scanning all the supplied types; 

*NOTE*: The supplied types must also contain all the NServiceBus types;

**NOTE**:

* Subsequent calls to the `With` method are idempotent and only one configuration is created;
* The `With` method (and in general the whole configuration API) is not thread safe; when you configure the entry point, make sure it is thread safe, based on the host used:
	* For `IIS`, configure NServiceBus in the `Application_Start()` method;
	* For `OWIN`, configure NServiceBus in the `Startup()` method;
	* For self-hosted `WCF` services, configure NServiceBus before opening the `ServiceHost`;