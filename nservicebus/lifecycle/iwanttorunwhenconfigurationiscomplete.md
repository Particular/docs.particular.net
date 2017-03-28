---
title: When Configuration Ends
summary: An interface that supports hooking into the configuration sequence of NServiceBus.
reviewed: 2016-08-23
component: Core
versions: '[,6.0)'
tags:
 - life-cycle
related:
 - samples/startup-shutdown-sequence
---

NOTE: This interface is deprecated in Version 6 and has been replaced with [Features](/nservicebus/pipeline/features.md).

Classes that implement `IWantToRunWhenConfigurationIsComplete` are executed when configuration is completed, right before the bus is created. Use `IWantToRunWhenConfigurationIsComplete` for any tasks that need to be run at the end of bus creation life-cycle. This might include logging diagnostic information about configuration or initialization logic that downstream components depend on.


Instances are:

 * Located by [assembly scanning](/nservicebus/hosting/assembly-scanning.md) and automatically registered into the [configured container](/nservicebus/containers/) during bus creation. These are registered as Instance Per Call.
 * Created as the last step before the bus is created.
 * Created on the same thread that is creating the bus.
 * Created by the [Container](/nservicebus/containers/) which means they:
  * Will have dependencies injected.
  * Do not require a default constructor.

partial: order

Once created `Run(...)` is called on each instance. These calls are made sequentially on the thread that is creating the bus. The order of these calls is determined by the order in which the configured container returns instances.

Exceptions thrown by instances of `IWantToRunWhenConfigurationIsComplete` are not handled by NServiceBus. These will bubble up to the creator of the bus.

snippet: lifecycle-iwanttorunwhenconfigurationiscomplete