---
title: IWantToRunWhenConfigurationIsComplete
summary: An interface that allows you to hook into the configuration sequence of NServiceBus
tags:
 - life cycle
related:
 - samples/startup-shutdown-sequence
---

Classes that implement `IWantToRunWhenConfigurationIsComplete` are executed when configuration is completed, right before the bus is created. Use `IWantToRunWhenConfigurationIsComplete` for any tasks that need to be run at the end of bus creation lifecycle. This might include logging diagnostic information about configuration or initialization logic that downstream components depend on.

NOTE: This interface is deprecated in Version 6.

Instances are:
* located by [assembly scanning](/nservicebus/hosting/assembly-scanning.md) and automatically registered into the [configured container](/nservicebus/containers/index.md) during bus creation. These are registered as Instance Per Call.
* created as the last step before the bus is created. 
* created on the same thread that is creating the bus.
* created by the configured container which means they:
  * will have dependencies injected.
  * do not require a default constructor.

NOTE:In Version 3 and Version 4, instances of `IWantToRunWhenConfigurationIsComplete` are registered in the configured container before instances of `INeedInitialization` are created and run. In Version 5 `INeedInitialization` happens first.

Once created `Run(...)` is called on each instance. These calls are made sequentially on the thread that is creating the bus. The order of these calls is determined by the order in which the configured container returns instances.

Exceptions thrown by instances of `IWantToRunWhenConfigurationIsComplete` are unhandled by NServiceBus. These will bubble up to the creator of the bus.

<!-- import lifecycle-iwanttorunwhenconfigurationiscomplete -->

