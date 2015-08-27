---
title: IWantToRunBeforeConfigurationIsFinalized
summary: An interface that allows you to hook into the configuration sequence of NServiceBus
tags: 
 - life cycle
related:
 - samples/startup-shutdown-sequence
---

Classes that implement `IWantToRunBeforeConfigurationIsFinalized` are used to adjust the `Configure` instance which is used to construct the Bus. This occurs just before Feature Activation.

Instances are located during via Type Scanning just before they are invoked. This means that they may be 
Instances are created as one of the very last steps of Bus creation.
Instances are all created on the calling thread. Instances are created with `Activator.CreateInstance(...)` and require a default constructor. Instances will not have any dependencies injected. 
As instances are created `Run(Configure)` is called on each one in series. 
Exceptions thrown by instances of `IWantToRunBeforeConfigurationIsFinalized` are unhandled by NServiceBus. These will bubble up to the creator of the bus.

NOTE: After all instances of `IWantToRunBeforeConfigurationIsFinalized` are executed Features activation begins. During Feature activation the `Settings` object becomes readonly. Downstream components should only rely on the `NServiceBus.Settings.ReadOnlySettings` interface. 

Use `IWantToRunBeforeConfigurationIsFinalized` for any last minute alterations to `Configure.Settings` that are required for downstream components. This can include enabling features, setting or overriding default settings used by features. 