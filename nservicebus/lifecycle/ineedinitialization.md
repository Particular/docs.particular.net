---
title: Initialization
summary: An interface that supports hooking into the very beginning of the bus creation sequence of NServiceBus.
component: Core
reviewed: 2019-07-18
related:
 - samples/startup-shutdown-sequence
---

Classes that implement `NServiceBus.INeedInitialization` are created and called as one of the first steps in the bus creation lifecycle. Use `INeedInitialization` to register components that will be used later in the bus creation lifecycle.

partial: namespace

Instances are:

 * Located by [assembly scanning](/nservicebus/hosting/assembly-scanning.md).
 * Created as one of the very first steps when the bus is created.
 * Created on the same thread that is creating the bus.
 * Created with [`Activator.CreateInstance(...)`](https://msdn.microsoft.com/en-us/library/system.activator.createinstance) which means they:
    * Are not resolved from [dependency injection](/nservicebus/dependency-injection/) (even if they are registered there).
    * Will not have any dependencies injected.
    * Must have a default constructor.

All calls are made in sequence on the thread that is creating the bus. The order of these calls is determined by the order of the scanned types list as a result of the assembly scan.

Exceptions thrown by instances of `INeedInitialization` are not handled by NServiceBus. These will bubble up to the caller creating the endpoint.

NOTE: Instances of `INeedInitialization` are created after type-scanning has occurred. Do not attempt to alter the types to be scanned from an instance of `INeedInitialization`.

snippet: lifecycle-ineedinitialization