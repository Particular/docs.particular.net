---
title: Before Configuration Finalized
summary: An interface that allows hooking into the configuration sequence of NServiceBus
component: Core
reviewed: 2024-08-01
related:
 - samples/startup-shutdown-sequence
---

> [!CAUTION]
> `IWantToRunBeforeConfigurationIsFinalized` is deprecated starting in NServiceBus version 10. Refer to the [NServiceBus 10 upgrade guide](/nservicebus/upgrades/9to10/#deprecated-iwanttorunbeforeconfigurationisfinalized).

During endpoint creation the configuration object used to construct the endpoint becomes frozen and locked. Classes that implement `IWantToRunBeforeConfigurationIsFinalized` are instantiated and called just before this happens. Use `IWantToRunBeforeConfigurationIsFinalized` for any last minute alterations to the configuration that may rely on other configuration settings.

Instances are:

 * Located by [assembly scanning](/nservicebus/hosting/assembly-scanning.md).
 * Created just before the configuration is frozen.
 * Created on the same thread that is creating the bus.
 * Created with [`Activator.CreateInstance(...)`](https://msdn.microsoft.com/en-us/library/system.activator.createinstance) which means they:
    * Are not resolved by [dependency injection](/nservicebus/dependency-injection/) (even if they are registered there).
    * Will not have any dependencies injected.
    * Must have a default constructor.

Once instantiated, `Run(...)` is called on each instance. These calls are made on the same thread that is creating the endpoint.  The order in which instances are instantiated and run is non-deterministic and should not be relied upon.

Exceptions thrown by instances of `IWantToRunBeforeConfigurationIsFinalized` are unhandled by NServiceBus and will bubble up to the caller.

snippet: lifecycle-iwanttorunbeforeconfigurationisfinalized
