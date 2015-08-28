---
title: IWantToRunBeforeConfigurationIsFinalized
summary: An interface that allows you to hook into the configuration sequence of NServiceBus
tags: 
 - life cycle
related:
 - samples/startup-shutdown-sequence
---

During bus creation the configuration object used to construct the bus becomes frozen and locked. Classes that implement `IWantToRunBeforeConfigurationIsFinalized` are created and called just before this happens. Use `IWantToRunBeforeConfigurationIsFinalized` for any last minute alterations to the configuration that may rely on other configuration settings. 

Instances are:
* located by [assembly scanning](/nservicebus/hosting/assembly-scanning.md).
* created just before the configuration is frozen.
* created on the same thread that is creating the bus. Instances are created in the order they appear in the scanned types list as a result of the assembly scan.
* created with [`Activator.CreateInstance(...)`](https://msdn.microsoft.com/en-us/library/system.activator.createinstance) which means they:
  * are not resolved out of an IoC container (even if they are registered there).
  * will not have any dependencies injected.
  * must have a default constructor.

As instances are created `Run(...)` is called. These calls are made in sequence on the thread that is creating the bus.

Exceptions thrown by instances of `IWantToRunBeforeConfigurationIsFinalized` are unhandled by NServiceBus. These will bubble up to the caller creating the bus.

<!-- import lifecycle-iwanttorunbeforeconfigurationisfinalized -->