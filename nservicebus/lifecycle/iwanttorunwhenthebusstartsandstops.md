---
title: When Configuration Start and Stop
summary: An interface that allows you to hook into the startup and shutdown sequence of NServiceBus
tags:
 - life-cycle
related:
 - samples/startup-shutdown-sequence
---

Classes that implement `IWantToRunWhenTheBusStartsAndStops` are invoked just after the bus has been started and just before it is stopped. Use `IWantToRunWhenTheBusStartsAndStops` for any tasks that need to execute with the same lifecycle as the bus.

Instances are:
* Located by [assembly scanning](/nservicebus/hosting/assembly-scanning.md) and automatically registered into the [configured container](/nservicebus/containers/) during bus creation. These are registered as Instance Per Call. 
* Created and started as the last step when the bus is started.
* Started after the Transport and any Satellites have started. You cannot rely on all instances of `IWantToRunWhenTheBusStartsAndStops` being created or started before messages start to be processed by your endpoint.
* Created on the same thread that is starting the bus.
* Created by the configured container which means they:
  * Will have dependencies injected.
  * Do not require a default constructor.

Exceptions thrown in the constructors of instances of `IWantToRunWhenTheBusStartsAndStops` are unhandled by NServiceBus. These will bubble up to the code that starts the the bus.

Once created `Start()` is called on each instance in parallel. Each `Start()` call is made on a different background thread. Instances of `IWantToRunWhenTheBusStartsAndStops` are kept internally to be stopped when the bus is disposed.

Exceptions raised from the `Start()` method will cause a [Critical Error](/nservicebus/hosting/critical-errors.md). As they are run on separate threads, an exception on one `Start()` call will not interfere with any others.

NOTE: The call to `IStartableBus.Start()` may return before all instances of `IWantToRunWhenTheBusStartsAndStops.Start()` are completed.

When the Bus is disposed, all instances of `IWantToRunWhenTheBusStartsAndStops` are stopped by calling their `Stop()` method. Each call to `Stop()` happens on parallel background threads but the call to `bus.Dispose()` will block until they have all completed. Any exceptions thrown by a call to `Stop` will be logged at the Fatal level.

NOTE: `Stop()` will wait for any outstanding instances of `Start()` to complete. If an instance of `IWantToRunWhenTheBusStartsAndStops` needs to be long running then it must start it's own background thread. Failure to do so will prevent the bus from being disposed.

NOTE: The bus keeps an internal list of instances which need to be stopped but the instances are registered with the `Builder` as Instance Per Call by default. This means that any attempt to resolve your own instance via dependency injection will result in a new instance and not the one which has been started and which will be stopped.

<!-- import lifecycle-iwanttorunwhenthebusstartsandstops -->