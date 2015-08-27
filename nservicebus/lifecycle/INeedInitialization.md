---
title: INeedInitialization
summary: An interface that allows you to hook into the configuration sequence of NServiceBus
related:
 - samples/startup-shutdown-sequence
---

Classes that implement `INeedInitialization` are used to adjust the `BusConfiguration` instance which is used to construct the Bus.

Instances are located during Type Scanning. 
Instances are created as one of the very first steps when `Bus.Create()` or `Bus.CreateSendOnly()` is called.
Instances are all created on the calling thread. Instances are created with `Activator.CreateInstance(...)` and require a default constructor. Instances will not have any dependencies injected. 
As instances are created `Customize(BusConfiguration)` is called on each one in series. 
Exceptions thrown by instances of `INeedInitialization` are unhandled by NServiceBus. These will bubble up to the caller of `Bus.Create()` or `Bus.CreateSendOnly()`.

NOTE: Instances of `INeedInitialization` are created after immediately after type-scanning has occurred. You should not call `TypesToScan`, `AssembliesToScan`, or `ScanAssembliesInDirectory` from an instance of `INeedInitialization`.

Use `INeedInitialization` to alter the `BusConfiguration` instance as early as possible in the bus lifecyce process. Frequently this is used to register components that will be used later in the lifecycle. 