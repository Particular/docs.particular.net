---
title: Custom Endpoint Initialization
summary: Implement INeedInitialization to hook into the very beginning of the endpoint creation sequence of NServiceBus.
component: Core
reviewed: 2024-08-05
related:
 - samples/startup-shutdown-sequence
---

Classes that implement `NServiceBus.INeedInitialization` are created and called as one of the first steps performed during endpoint creation. Use `INeedInitialization` to register components that will be used later in the endpoint creation sequence.

Instances are:

 * Located by [assembly scanning](/nservicebus/hosting/assembly-scanning.md).
 * Created as one of the first steps when the bus is created.
 * Created on the same thread that is creating the bus.
 * Created with [`Activator.CreateInstance(...)`](https://msdn.microsoft.com/en-us/library/system.activator.createinstance) which means they:
    * Are not resolved from [dependency injection](/nservicebus/dependency-injection/) (even if they are registered there).
    * Will not have any dependencies injected.
    * Must have a default constructor.

Once instantiated, `Customize(...)` is called on each instance. These calls are made on the same thread that is creating the endpoint.  The order in which instances are instantiated, and run is non-deterministic and should not be relied upon.

Exceptions thrown by instances of `INeedInitialization` are not handled by NServiceBus and will bubble up to the caller, creating the endpoint.

> [!NOTE]
> Instances of `INeedInitialization` are created after type-scanning has occurred. Do not attempt to alter the types to be scanned from an instance of `INeedInitialization`.

snippet: lifecycle-ineedinitialization
