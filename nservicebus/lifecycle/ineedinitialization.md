---
title: Initialization
summary: An interface that supports hooking into the very beginning of the bus creation sequence of NServiceBus.
reviewed: 2016-03-17
tags:
 - life-cycle
related:
 - samples/startup-shutdown-sequence
---

Classes that implement `NServiceBus.INeedInitialization` are created and called as one of the first steps in the bus creation life-cycle. Use `INeedInitialization` to register components that will be used later in the bus creation life-cycle.

NOTE: In Version 3 of NServiceBus, this interface is found in the `NServiceBus.Config` namespace. In Version 4 both interfaces are available but the old one is marked as obsolete. As of Version 5 use of the `NServiceBus.Config.INeedInitialization` interface will cause a compile-time error. In Version 4 (which has both interfaces), all instances of `NServiceBus.Config.INeedInitialization` are created and called and then all instances of `NServiceBus.INeedInitialization` are created and called.

Instances are:

 * Located by [assembly scanning](/nservicebus/hosting/assembly-scanning.md).
 * Created as one of the very first steps when the bus is created.
 * Created on the same thread that is creating the bus.
 * Created with [`Activator.CreateInstance(...)`](https://msdn.microsoft.com/en-us/library/system.activator.createinstance) which means they
  * Are not resolved out of the [Container](/nservicebus/containers/) (even if they are registered there).
  * Will not have any dependencies injected.
  * Must have a default constructor.

All calls are made in sequence on the thread that is creating the bus. The order of these calls is determined by the order of the scanned types list as a result of the assembly scan.

Exceptions thrown by instances of `INeedInitialization` are unhandled by NServiceBus. These will bubble up to the caller creating the endpoint.

NOTE: Instances of `INeedInitialization` are created after type-scanning has occurred. Do not attempt to alter the types to be scanned from an instance of `INeedInitialization`.

snippet:lifecycle-ineedinitialization