---
title: IWantToRunWhenTheBusStartsAndStops
summary: An interface that allows you to hook into the startup and shutdown sequence of NServiceBus
related:
 - samples/startup-shutdown-sequence
---

Classes that implement `IWantToRunWhenTheBusStartsAndStops` are invoked just after the bus has been started and just before it is stopped.

Instances are added to the Builder during bus creation using an Instance Per Call lifecyle.
Instances are all created on the thread responsible for starting the bus. Instances are created with the `Builder` instance and do not require a default constuctor. Instances will have any dependencies injected. 
Exceptions thrown in the constructors of instances of `IWantToRunWhenTheBusStartsAndStops` are unhandled by NServiceBus. These will bubble up to the code that starts the the bus.

NOTE: The transport is started before any instances of `IWantToRunWhenTheBusStartsAndStops` are created. You cannot rely on all instances of `IWantToRunWhenTheBusStartsAndStops` being created or started before messages start to be processed. 

Once created `Start()` is called on each instance in parallel. Each `Start()` call is made different background thread. Instances of `IWantToRunWhenTheBusStartsAndStops` are kept internally to be stopped when the bus is disposed.
Exceptions raised from the `Start()` method will cause a [Critical Error](/nservicebus/hosting/critical-errors). As they are run on separate threads, an exception on one `Start()` call will not interfere with any others.

NOTE: The call to `IStartable.Start()` may return before all instances of `IWantToRunWhenTheBusStartsAndStops.Start()` are completed.

NOTE: Feature Startup Tasks are run from an instance of `IWantToRunWhenTheBusStartsAndStops`. This means that other instances of `IWantToRunWhenTheBusStartsAndStops` cannot rely on Feature Startup Tasks completing before they are called.

When the Bus is disposed, all instances of `IWantToRunWhenTheBusStartsAndStops` are stopped by calling their `Stop()` method. Each call to `Stop()` happens on parallel background threads but the call to `bus.Dispose()` will not return until they have all completed. Any exceptions thrown by a call to `Stop` will be logged at the Fatal level.

NOTE: `Stop()` will wait for any outstanding instances of `Start()` to complete. If an instance of `IWantToRunWhenTheBusStartsAndStops` needs to be long running then it should start it's own background thread. Failure to do so will prevent the bus from disposed.

NOTE: The bus keeps an internal list of instances which need to be stopped but the instances are registered with the `Builder` as Instance Per Call by default. This means that any attempt to resolve your own instance via dependency injection will result in a new instance and not the one which has been started and which will be stopped.

Use `IWantToRunWhenTheBusStartsAndStops` for any tasks that need to execute with the same lifecycle as the bus.