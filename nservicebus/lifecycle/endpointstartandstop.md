---
title: When Endpoint Instance Starts and Stops
summary: An interface that allows hooking into into the startup and shutdown sequence of an endpoint instance.
reviewed: 2016-03-30
tags:
 - life-cycle
related:
 - samples/startup-shutdown-sequence
redirects:
 - nservicebus/lifecycle/iwanttorunwhenbusstartsandstops
---

Classes that plug into the startup/shutdown sequence are invoked just after the endpoint instance has been started and just before it is stopped. Use this approach for any tasks that need to execute with the same life-cycle as the endpoint instance.

NOTE: The endpoint instance keeps an internal list of instances which need to be stopped but the instances are registered with the `Builder` as Instance Per Call by default. This means that any attempt to resolve a custom instance via dependency injection will result in a new instance and not the one which has been started and which will be stopped.

## Versions 6 and above

The `IWantToRunWhenEndpointStartsAndStops` interface was previously available within the NServiceBus core library as `IWantToRunWhenBusStartsAndStops`. This interface is only available for those using the [NServiceBus.Host](https://www.nuget.org/packages/NServiceBus.Host/). For more information about the NServiceBus.Host, see the [documentation for the host](/nservicebus/hosting/nservicebus-host).

Instances are:

 * Located by [assembly scanning](/nservicebus/hosting/assembly-scanning.md) and automatically registered into the [configured container](/nservicebus/containers/) during bus creation. These are registered as `Instance Per Call`.
 * Created and Started before the Transport and any Satellites have started. This means no messages will be sent until after all `Start` methods have been called.
 * Created in the same method that starts the endpoint.
 * Created by the [Container](/nservicebus/containers/) which means they:
  * Will have dependencies injected.
  * Do not require a default constructor.
 * Started asynchronously in the same method which started the bus.
 * Stopped after the Transport and any Satellites have stopped. While all instances of `IWantToRunWhenEndpointStartsAndStops` are being stopped, no messages will be handled.
 * Stopped asynchronously within the same method which disposed the bus.

Once created `Start` is called on each instance asynchronously without awaiting its completion. Each call to `Start` is called in the `OnStart` method of a [FeatureStartupTask](/nservicebus/pipeline/features.md#feature-startup-tasks). 

Instances of `IWantToRunWhenEndpointStartsAndStops` which successfully start are kept internally to be stopped when the bus is stopped.

NOTE: The call to `IStartableEndpoint.Start` will not return before all instances of `IWantToRunWhenEndpointStartsAndStops.Start` are completed.

NOTE: The `Start` and `Stop` methods will block start up and shut down of the endpoint. For any long running methods, use `Task.Run` so as not to block execution.

When the endpoint is shut down, the `Stop` method for each instance of `IWantToRunWhenEndpointStartsAndStops` is called asynchronously. Each call to `Stop` is called in the `OnStop` method of a [FeatureStartupTask](/nservicebus/pipeline/features.md#feature-startup-tasks).

include: non-null-task

### Exceptions

Exceptions thrown in the constructors of instances of `IWantToRunWhenEndpointStartsAndStops` are unhandled by NServiceBus. These will prevent the endpoint from starting up.

Exceptions raised from the `Start` method will prevent the endpoint from starting. As they are called asynchronously and awaited with `Task.WhenAll`, an exception may prevent other `Start` methods from being called. 

Exceptions raised from the `Stop` method will not prevent the endpoint from shutting down. The exceptions will be logged at the Fatal level.

## Versions 5 and below

Instances are:

 * Located by [assembly scanning](/nservicebus/hosting/assembly-scanning.md) and automatically registered into the [configured container](/nservicebus/containers/) during bus creation. These are registered as `Instance Per Call`.
 * Created and started as the last step when the bus is started.
 * Started after the Transport and any Satellites have started. While all instances of `IWantToRunWhenBusStartsAndStops` are being started, no message will be processed.
 * Created on the same thread that is starting the bus.
 * Created by the [Container](/nservicebus/containers/) which means they:
  * Will have dependencies injected.
  * Do not require a default constructor.

Once created `Start()` is called on each instance in parallel. Each `Start()` call is made on a different background thread. Instances of `IWantToRunWhenBusStartsAndStops` are kept internally to be stopped when the bus is disposed.

NOTE: The call to `IStartableBus.Start()` may return before all instances of `IWantToRunWhenBusStartsAndStops.Start()` are completed.

When the Bus is disposed, all instances of `IWantToRunWhenBusStartsAndStops` are stopped by calling their `Stop` method. Each call to `Stop` happens on parallel background threads but the call to `bus.Dispose()` will block until they have all completed. 

NOTE: `Stop()` will wait for any outstanding instances of `Start()` to complete. If an instance of `IWantToRunWhenBusStartsAndStops` needs to be long running then it must start it's own background thread. Failure to do so will prevent the bus from being disposed.

### Exceptions

Exceptions thrown in the constructors of instances of the start and stop interfaces are unhandled by NServiceBus. These will bubble up to the code that starts the the bus.

Exceptions raised from the `Start` method will cause a [Critical Error](/nservicebus/hosting/critical-errors.md). As they are run on separate threads, an exception on one `Start()` call will not interfere with any others.

Exceptions raised from the `Stop` method will not halt shutdown and will be logged at the Fatal level.

## Code

snippet:lifecycle-EndpointStartAndStop
