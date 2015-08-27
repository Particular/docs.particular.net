---
title: IWantToRunWhenConfigurationIsComplete
summary: An interface that allows you to hook into the configuration sequence of NServiceBus
tags:
 - life cycle
related:
 - samples/startup-shutdown-sequence
---

Classes that implement `IWantToRunWhenConfigurationIsComplete` are executed right before the bus is created.

Instances are located during Type Scanning. Alternatively they may be added by instances of `INeedInitialization` or `IWantToRunBeforeConfigurationIsFinalized`.
Instances are all created on the thread responsible for creating the bus. Instances are created with the `Builder` instance and do not require a default constructor. Instances will have any dependencies injected. 
Once created `Run(Configure)` is called on each one in series. 
Exceptions thrown by instances of `IWantToRunWhenConfigurationIsComplete` are unhandled by NServiceBus. These will bubble up to the creator of the bus.

Use `IWantToRunWhenConfigurationIsComplete` for any tasks that need to be run at the end of bus creation lifecycle. This might include diagnostic information about configuration or initialization that downstream Features and components might depend on.