---
title: Before Configuration Finalized
summary: An interface that allows hooking into the configuration sequence of NServiceBus.
component: Core
reviewed: 2019-07-22
related:
 - samples/startup-shutdown-sequence
---

During bus creation the configuration object used to construct the bus becomes frozen and locked. Classes that implement `IWantToRunBeforeConfigurationIsFinalized` are created and called just before this happens. Use `IWantToRunBeforeConfigurationIsFinalized` for any last minute alterations to the configuration that may rely on other configuration settings.

Instances are:

 * Located by [assembly scanning](/nservicebus/hosting/assembly-scanning.md).
 * Created just before the configuration is frozen.
 * Created on the same thread that is creating the bus.
 * Created with [`Activator.CreateInstance(...)`](https://msdn.microsoft.com/en-us/library/system.activator.createinstance) which means they:
    * Are not resolved by [dependency injection](/nservicebus/dependency-injection/) (even if they are registered there).
    * Will not have any dependencies injected.
    * Must have a default constructor.

Once created `Run(...)` is called on each instance. These calls are made sequentially on the thread that is creating the bus. The order of these calls is determined by the order of the scanned types list as a result of the assembly scan.

Exceptions thrown by instances of `IWantToRunBeforeConfigurationIsFinalized` are unhandled by NServiceBus. These will bubble up to the caller creating the bus.

snippet: lifecycle-iwanttorunbeforeconfigurationisfinalized